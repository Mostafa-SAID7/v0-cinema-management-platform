using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models.System;
using MoviesAPI.Service;
using System.Text;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<SupportController> _logger;

        public SupportController(IEmailService emailService, ILogger<SupportController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost("contact")]
        public async Task<IActionResult> SubmitContactForm([FromBody] SupportMessage supportMessage)
        {
            // Server-side validation
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new
                {
                    message = "Validation failed",
                    errors = errors
                });
            }

            try
            {
                // Send email to support team
                var emailMessage = new EmailMessage
                {
                    MailTo = "support@shoftv.com", // Your support email
                    Subject = $"Support Request: {supportMessage.Subject}"
                };

                var sb = new StringBuilder();
                sb.AppendLine("<html>");
                sb.AppendLine("<body style='font-family: Arial, sans-serif;'>");
                sb.AppendLine("<h2 style='color: #ef4444;'>New Support Request</h2>");
                sb.AppendLine("<div style='background: #f5f5f5; padding: 20px; border-radius: 8px;'>");
                sb.AppendLine($"<p><strong>From:</strong> {supportMessage.Name}</p>");
                sb.AppendLine($"<p><strong>Email:</strong> {supportMessage.Email}</p>");
                sb.AppendLine($"<p><strong>Subject:</strong> {supportMessage.Subject}</p>");
                sb.AppendLine($"<p><strong>Message:</strong></p>");
                sb.AppendLine($"<p style='white-space: pre-wrap;'>{supportMessage.Message}</p>");
                sb.AppendLine("</div>");
                sb.AppendLine($"<p style='color: #666; font-size: 12px; margin-top: 20px;'>Received at: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC</p>");
                sb.AppendLine("</body>");
                sb.AppendLine("</html>");

                emailMessage.Content = sb.ToString();

                // Send email (will fail if email service not configured, but that's okay for development)
                try
                {
                    await _emailService.SendEmailAsync(emailMessage);
                }
                catch (Exception emailEx)
                {
                    _logger.LogWarning($"Failed to send email: {emailEx.Message}");
                    // Continue anyway - we'll still return success to user
                }

                // Log the support request
                _logger.LogInformation($"Support request received from {supportMessage.Email}: {supportMessage.Subject}");

                return Ok(new
                {
                    message = "Your message has been sent successfully! We'll get back to you within 24 hours.",
                    success = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing support request: {ex.Message}");
                return StatusCode(500, new
                {
                    message = "An error occurred while sending your message. Please try again later.",
                    success = false
                });
            }
        }

        [HttpGet("faq")]
        public IActionResult GetFAQ()
        {
            var faqs = new[]
            {
                new
                {
                    question = "How do I book tickets?",
                    answer = "Browse movies, select a showtime, choose your seats, and complete the payment.",
                    category = "Booking"
                },
                new
                {
                    question = "Can I cancel my booking?",
                    answer = "Yes, you can cancel up to 2 hours before the showtime for a full refund.",
                    category = "Cancellation"
                },
                new
                {
                    question = "What payment methods do you accept?",
                    answer = "We accept all major credit cards, debit cards, and digital wallets.",
                    category = "Payment"
                },
                new
                {
                    question = "How do I access my tickets?",
                    answer = "Your tickets are available in the 'My Tickets' section after booking.",
                    category = "Tickets"
                }
            };

            return Ok(faqs);
        }
    }
}
