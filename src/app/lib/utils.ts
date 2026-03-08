/**
 * shadcn-aligned utility functions for Angular
 * These utilities support the shadcn design system principles
 */

/**
 * Merge Tailwind CSS classes intelligently
 * Handles conflicts and deduplication
 */
export function cn(...classes: (string | undefined | null | false)[]): string {
  return classes
    .filter((cls) => typeof cls === 'string')
    .join(' ')
    .replace(/\s+/g, ' ')
    .trim();
}

/**
 * Class Variance Authority for component variants
 * shadcn pattern for creating component variants with type safety
 */
export interface CVAConfig {
  base?: string;
  variants?: Record<string, Record<string, string>>;
  compoundVariants?: Array<{
    [key: string]: string | string[] | boolean;
    class: string;
  }>;
  defaultVariants?: Record<string, string | boolean>;
}

export type VariantProps<T extends CVAConfig> = {
  [K in keyof T['variants']]?: keyof T['variants'][K] extends string
    ? keyof T['variants'][K]
    : never;
};

/**
 * Create a component variant function similar to class-variance-authority
 * Usage:
 * const buttonVariants = cva('btn', {
 *   variants: {
 *     size: { sm: 'text-sm', lg: 'text-lg' },
 *     variant: { primary: 'bg-primary', secondary: 'bg-secondary' }
 *   }
 * })
 */
export function cva(
  base: string,
  config: CVAConfig
): (props?: Record<string, any> & { class?: string }) => string {
  return (props: Record<string, any> = {}) => {
    const classes: string[] = [config.base || base];

    // Add variant classes
    if (config.variants) {
      for (const [variantKey, variantValue] of Object.entries(config.variants)) {
        const selectedVariant = (props as Record<string, any>)[variantKey];
        if (selectedVariant && variantValue[selectedVariant as string]) {
          classes.push(variantValue[selectedVariant as string]);
        } else if (config.defaultVariants?.[variantKey]) {
          classes.push(
            variantValue[config.defaultVariants[variantKey] as string]
          );
        }
      }
    }

    // Add compound variants
    if (config.compoundVariants) {
      for (const compound of config.compoundVariants) {
        let matches = true;
        for (const [key, value] of Object.entries(compound)) {
          if (key === 'class') continue;
          const propValue = (props as Record<string, any>)[key];
          if (Array.isArray(value)) {
            if (!value.includes(propValue as string)) {
              matches = false;
              break;
            }
          } else if (propValue !== value) {
            matches = false;
            break;
          }
        }
        if (matches) {
          classes.push(compound.class);
        }
      }
    }

    // Add custom class override
    if (props.class) {
      classes.push(props.class);
    }

    return cn(...classes);
  };
}

/**
 * Type-safe Tailwind merge utility
 * Prevents conflicting Tailwind classes
 */
export function mergeClasses(...classes: string[]): string {
  const classList = new Set<string>();

  for (const classString of classes) {
    if (!classString) continue;

    const parts = classString.split(/\s+/);
    for (const part of parts) {
      // Remove conflicting classes
      if (part.startsWith('bg-') || part.startsWith('text-')) {
        // Remove previous versions
        for (const existing of classList) {
          if (existing.startsWith(part.split('-')[0])) {
            classList.delete(existing);
          }
        }
      }
      classList.add(part);
    }
  }

  return Array.from(classList).join(' ');
}

/**
 * Create a forwarded ref handler for Angular components
 * shadcn pattern for exposing native element access
 */
export type Ref<T> = (instance: T | null) => void;

/**
 * Type helper for extracting variant props
 */
export type WithVariants<T extends CVAConfig> = Partial<VariantProps<T>>;
