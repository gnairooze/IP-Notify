# IP-Notify

This project main goal is to send the public IP of the current machine and email it.

It uses the following nuget packages:
1. SendGrid to send emails.
2. RestSharp as an IP client.
3. Serilog for logging.

---

## Prerequisites

To run this tool, the following system enviornment variables have to be declared:
1. SENDGRID_API_KEY
2. SENDGRID_EMAIL
3. SENDGRID_NAME

