# Online Auction Microservice System

## Problem Statement

The online auction market requires an improved communication system to enable effective real-time interactions among users during bidding. The system should:

- Allow users to enter a bidding room.
- Facilitate bid submissions.
- Notify the highest bidder at the end of the auction.
- Automatically create an invoice for the highest bidder.

## Tasks

### 1. API Development

- Develop an API for the basic operations of the auction platform.
- The API should handle user interactions with the bidding room and manage bid submissions.

### 2. Messaging System Implementation

Implement a messaging queuing system to facilitate communication between different services. We will use RabbitMQ for this purpose.

#### a. Room Service to Bidding Service

- **Objective**: When a user enters a bidding room and the auction starts, the Room Service sends a message to the Bidding Service.
- **Purpose**: Activate and monitor the bidding process. Ensure the Bidding Service is ready to receive and process bids.

#### b. Bidding Service to Notification Service

- **Objective**: As bids are submitted and updated, the Bidding Service communicates with the Notification Service.
- **Purpose**: Send real-time updates or alerts to all participants in the auction room, keeping them informed of the current highest bid and other relevant events.

#### c. Notification Service to Invoice Service

- **Objective**: Once the auction concludes, the Notification Service communicates with the Invoice Service.
- **Purpose**: Initiate the generation of an invoice for the highest bidder, detailing the winning bid and the item purchased.

#### d. Invoice Service to Payment Service

- **Objective**: The Invoice Service sends the invoice details to the Payment Service.
- **Purpose**: Process the payment from the highest bidder based on the invoice. Handle transaction details, confirm payment, and update the auction system upon successful completion of the transaction.

## Technical Stack

- **Backend Framework**: .NET 8 (or the latest version)
- **Messaging Queue**: RabbitMQ
- **Database**: SQL Server / PostgreSQL / MongoDB
- **Other Tools**: Docker (for containerization), Swagger (for API documentation)

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) - Install the .NET SDK if not already installed.
- [RabbitMQ](https://www.rabbitmq.com/download.html) - Set up RabbitMQ server.
- [Docker](https://www.docker.com/get-started) - (Optional) Install Docker if you want to use it for running RabbitMQ.

### Installation

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/donkennie/auction-microservice-platform.git
   cd auction-microservice-platform
