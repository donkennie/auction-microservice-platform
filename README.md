graph TD
    A[API Gateway] --> B[Room Service]
    A --> C[Bidding Service]
    A --> D[Notification Service]
    A --> E[Invoice Service]
    A --> F[Payment Service]
    B <--> G[RabbitMQ]
    C <--> G
    D <--> G
    E <--> G
    F <--> G
    H[User] --> A
