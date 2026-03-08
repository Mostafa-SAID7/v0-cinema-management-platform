/**
 * shadcn Component Registry
 * Centralizes component metadata and patterns
 * Enables consistency across the application
 */

export interface ComponentMetadata {
  name: string;
  description: string;
  category: 'input' | 'feedback' | 'layout' | 'overlay' | 'navigation' | 'data';
  a11y: {
    waiAria: string[];
    keyboardSupport: string[];
    screenReaderTested: boolean;
  };
  variants?: string[];
  slots?: string[];
  dependencies?: string[];
  status: 'stable' | 'beta' | 'deprecated';
}

/**
 * Registry of all shadcn-aligned components
 */
export const COMPONENT_REGISTRY: Record<string, ComponentMetadata> = {
  // Input Components
  button: {
    name: 'Button',
    description: 'Clickable element with variants and states',
    category: 'input',
    a11y: {
      waiAria: ['role="button"', 'aria-pressed', 'aria-disabled'],
      keyboardSupport: ['Enter', 'Space'],
      screenReaderTested: true,
    },
    variants: ['default', 'secondary', 'destructive', 'outline', 'ghost', 'link'],
    status: 'stable',
  },
  input: {
    name: 'Input',
    description: 'Text input field with validation support',
    category: 'input',
    a11y: {
      waiAria: ['aria-label', 'aria-describedby', 'aria-invalid'],
      keyboardSupport: ['Tab', 'All text input keys'],
      screenReaderTested: true,
    },
    slots: ['prefix', 'suffix'],
    status: 'stable',
  },
  'form-field': {
    name: 'Form Field',
    description: 'Compound component for form input with label and error',
    category: 'input',
    a11y: {
      waiAria: ['aria-labelledby', 'aria-describedby'],
      keyboardSupport: [],
      screenReaderTested: true,
    },
    slots: ['label', 'input', 'error', 'description'],
    dependencies: ['input'],
    status: 'stable',
  },
  select: {
    name: 'Select',
    description: 'Dropdown selection component',
    category: 'input',
    a11y: {
      waiAria: ['role="combobox"', 'aria-expanded', 'aria-owns'],
      keyboardSupport: ['Enter', 'Space', 'ArrowUp', 'ArrowDown', 'Escape'],
      screenReaderTested: true,
    },
    slots: ['trigger', 'content', 'item'],
    status: 'beta',
  },
  checkbox: {
    name: 'Checkbox',
    description: 'Boolean input element',
    category: 'input',
    a11y: {
      waiAria: ['role="checkbox"', 'aria-checked'],
      keyboardSupport: ['Space'],
      screenReaderTested: true,
    },
    status: 'stable',
  },
  'radio-group': {
    name: 'Radio Group',
    description: 'Single selection from multiple options',
    category: 'input',
    a11y: {
      waiAria: ['role="radiogroup"', 'role="radio"', 'aria-checked'],
      keyboardSupport: ['ArrowUp', 'ArrowDown', 'ArrowLeft', 'ArrowRight'],
      screenReaderTested: true,
    },
    slots: ['item'],
    status: 'stable',
  },

  // Feedback Components
  toast: {
    name: 'Toast',
    description: 'Non-blocking notification system',
    category: 'feedback',
    a11y: {
      waiAria: ['role="alert"', 'role="status"', 'aria-live="polite"'],
      keyboardSupport: [],
      screenReaderTested: true,
    },
    variants: ['default', 'success', 'error', 'warning', 'info'],
    status: 'stable',
  },
  'alert-dialog': {
    name: 'Alert Dialog',
    description: 'Modal dialog for critical actions',
    category: 'feedback',
    a11y: {
      waiAria: ['role="alertdialog"', 'aria-modal="true"'],
      keyboardSupport: ['Escape', 'Tab'],
      screenReaderTested: true,
    },
    slots: ['title', 'description', 'action'],
    status: 'stable',
  },

  // Layout Components
  card: {
    name: 'Card',
    description: 'Container with shadow and border styling',
    category: 'layout',
    a11y: {
      waiAria: [],
      keyboardSupport: [],
      screenReaderTested: false,
    },
    slots: ['header', 'title', 'content', 'footer'],
    status: 'stable',
  },
  separator: {
    name: 'Separator',
    description: 'Visual divider element',
    category: 'layout',
    a11y: {
      waiAria: ['role="separator"'],
      keyboardSupport: [],
      screenReaderTested: false,
    },
    status: 'stable',
  },

  // Overlay Components
  dialog: {
    name: 'Dialog',
    description: 'Modal dialog overlay',
    category: 'overlay',
    a11y: {
      waiAria: ['role="dialog"', 'aria-modal="true"', 'aria-labelledby'],
      keyboardSupport: ['Escape', 'Tab'],
      screenReaderTested: true,
    },
    slots: ['trigger', 'content', 'header', 'footer'],
    status: 'stable',
  },
  popover: {
    name: 'Popover',
    description: 'Non-modal overlay positioned relative to trigger',
    category: 'overlay',
    a11y: {
      waiAria: ['role="dialog"', 'aria-modal="false"'],
      keyboardSupport: ['Escape'],
      screenReaderTested: true,
    },
    slots: ['trigger', 'content'],
    status: 'beta',
  },
  tooltip: {
    name: 'Tooltip',
    description: 'Floating label with additional information',
    category: 'overlay',
    a11y: {
      waiAria: ['role="tooltip"', 'aria-describedby'],
      keyboardSupport: [],
      screenReaderTested: true,
    },
    status: 'stable',
  },

  // Navigation Components
  tabs: {
    name: 'Tabs',
    description: 'Tabbed content interface',
    category: 'navigation',
    a11y: {
      waiAria: ['role="tablist"', 'role="tab"', 'aria-selected', 'aria-controls'],
      keyboardSupport: ['ArrowLeft', 'ArrowRight', 'Home', 'End'],
      screenReaderTested: true,
    },
    slots: ['list', 'trigger', 'content'],
    status: 'stable',
  },
  accordion: {
    name: 'Accordion',
    description: 'Collapsible content sections',
    category: 'navigation',
    a11y: {
      waiAria: ['role="button"', 'aria-expanded', 'aria-controls'],
      keyboardSupport: ['Enter', 'Space', 'ArrowUp', 'ArrowDown'],
      screenReaderTested: true,
    },
    slots: ['item', 'trigger', 'content'],
    status: 'stable',
  },

  // Data Components
  table: {
    name: 'Table',
    description: 'Data table with sorting and pagination',
    category: 'data',
    a11y: {
      waiAria: [
        'role="table"',
        'role="row"',
        'role="columnheader"',
        'aria-sort',
      ],
      keyboardSupport: ['Tab', 'ArrowUp', 'ArrowDown'],
      screenReaderTested: true,
    },
    slots: ['header', 'body', 'footer', 'caption'],
    status: 'beta',
  },
};

/**
 * Get component metadata by name
 */
export function getComponentMetadata(componentName: string): ComponentMetadata | undefined {
  return COMPONENT_REGISTRY[componentName];
}

/**
 * Get all components in a category
 */
export function getComponentsByCategory(
  category: ComponentMetadata['category']
): [string, ComponentMetadata][] {
  return Object.entries(COMPONENT_REGISTRY).filter(
    ([, meta]) => meta.category === category
  );
}

/**
 * Validate component accessibility compliance
 */
export function validateA11y(componentName: string): {
  compliant: boolean;
  issues: string[];
} {
  const metadata = getComponentMetadata(componentName);
  if (!metadata) {
    return {
      compliant: false,
      issues: ['Component not found in registry'],
    };
  }

  const issues: string[] = [];

  if (metadata.a11y.waiAria.length === 0) {
    issues.push('No WAI-ARIA attributes defined');
  }

  if (!metadata.a11y.screenReaderTested) {
    issues.push('Not tested with screen readers');
  }

  return {
    compliant: issues.length === 0,
    issues,
  };
}
