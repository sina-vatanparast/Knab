# Answers to Technical Questions

## 1. How long did you spend on the coding assignment? What would you add to your solution if you had more time?

I spent a full day on the assignment, focusing on writing clean and maintainable code while ensuring it met the requirements. My primary goal was to deliver a high-quality solution that adhered to best practices.

If I had more time, I would have:
- Written more unit tests to cover edge cases and improve the overall robustness of the backend components.
- Enhanced exception handling to account for scenarios such as 3rd-party API timeouts and data deserialization failures.
- Cached the cryptocurrency list data, which rarely changes, either on the backend using decorators or on the frontend using local storage.
- Improved the frontend by making it more responsive and developing reusable components with frameworks like React or Angular.
- Introduced API call throttling to prevent exceeding rate limits and implemented retry mechanisms for transient failures.

## 2. What was the most useful feature that was added to the latest version of your language of choice? Please include a snippet of code that shows how you've used it.

One of the most useful features in the latest version of C# (.NET 8) is the `required` keyword for properties. This feature ensures that required properties must be set during object initialization, making the code safer and more predictable.

### Example Usage
```csharp
public class AppSettings
{
    public required ApiSettings CryptoCurrencyApi { get; set; }
    public required ApiSettings FiatCurrencyApi { get; set; }
    public required List<string> FiatCurrencyCodes { get; set; }
    public required string DefaultFiatCurrencyCode { get; set; }
    public int CryptoCurrenciesLimit { get; set; }
}

// Usage example
var appSettings = new AppSettings
{
    CryptoCurrencyApi = new ApiSettings { BaseUrl = "https://api.coinmarketcap.com", ApiKey = "b20758e6-a181-487e-9c79-fc9b26453ff2" },
    FiatCurrencyApi = new ApiSettings { BaseUrl = "https://api.exchangeratesapi.io", ApiKey = "3df3ba12170fa132f65accd56db5e00c" },
    FiatCurrencyCodes = new List<string> { "USD", "EUR", "GBP" },
    DefaultFiatCurrencyCode = "EUR",
    CryptoCurrenciesLimit = 10
};

// Fails at compile time if any required property is missing.
```
This feature has significantly improved the reliability of my code by catching potential initialization errors early.

## 3. How would you track down a performance issue in production? Have you ever had to do this?

When tracking down a performance issue in production, I focus on the key components and use both logging and monitoring tools to identify bottlenecks. For backend instances, I log request and response times for operations like database calls, message delivery, and external API requests. I set up warnings if the duration exceeds configurable thresholds, which helps pinpoint time-consuming operations. We often use APM tools like Dynatrace (as I did in a previous project) to monitor resources like CPU, memory, and response times. Additionally, health check endpoints are crucial for monitoring instance responsiveness.

If the issue involves message brokers, I enable broker logs and dead letter queues to monitor expired messages or failed acknowledgments. I also look at wire patterns to ensure the broker is receiving messages and check if receivers are overwhelmed or decoupled. In such cases, I use load leveling or load balancing to distribute the load.

For database-related performance issues, I rely on query profiling to identify slow queries and analyze database logs for signs of connection pool exhaustion or lock contention. If caching is involved, I use Redis Monitor to check cache miss rates and review server logs for connection issues or frequent evictions.

For network-related problems like latency or packet loss, APM tools and load balancer logs are invaluable. They help identify bottlenecks in traffic distribution or network flow, making it easier to diagnose and resolve issues.

This structured approach ensures that I can quickly identify and address the root causes of performance problems in production systems.

## 4. What was the latest technical book you have read or tech conference you have been to? What did you learn?

I have been reading the book *C# 13 and .NET 9 – Modern Cross-Platform Development Ninth Edition* by Mark J. Price. It has been insightful, particularly regarding updates on GraphQL support in the new C#. I’m currently implementing a GraphQL project using MongoDB, where flexible and powerful queries are critical. This book has enhanced my understanding of optimizing query efficiency and implementing best practices.

## 5. What do you think about this technical assessment?

The assessment was well-designed. I appreciated the limitations of free APIs, which required designing robust business logic to integrate different data schemas and provide a comprehensive DTO for the frontend. It also gave me an opportunity to implement Onion Architecture and encapsulate concerns, resulting in a modular and maintainable backend solution.

## 6. Please, describe yourself using JSON.

```json
{
  "personal_life": {
    "sports": {
      "table_tennis": [
        "competitions",
        "training"
      ],
      "gym": [
        "training"
      ]
    },
    "hobbies": [
      "movies",
      "hanging_out",
      "concerts",
      "catching_up_with_friends",
      "travels",
      "shopping"
    ],
    "daily_routines": {
      "sleeping": {},
      "household_chores": {
          "cooking": [],
          "cleaning": []
      },
      "commuting_to_work": {},
      "eating": {},
      "showering": {},
      "groceries": {}
    },
    "goals": [
      "financial_stability",
      "mental_stability",
      "emotional_stability",
      "building_a_lovely_family",
      "being_a_better_human_being"
    ],
    "family": [
      "communication_as_a_good_son",
      "communication_as_a_good_brother"
    ],
    "relationship": null,
    "guitar": [
      "playing_learned_songs",
      "learning_new_classical_songs"
    ]
  },
  "work_life": {
    "upskilling": [
      "soft_skills",
      "technical_skills"
    ],
    "career": [
      "conduct_duties_and_tasks",
      "meeting_deadlines",
      "high_quality_interaction_with_colleagues",
      "satisfying_work_ethics",
      "leading_subordinates_with_respect_and_support",
      "understanding_superior_and_align_the_team_direction_with_their_concerns"
    ],
    "goals": [
      "stability_in_company",
      "growth_as_team_member",
      "earning_trust",
      "accepting_more_responsibilities",
      "boosting_soft_skills",
      "proving_behavioural_and_technical_capabilities",
      "enhance_the_communication_with_colleagues_more_and_more"
    ]
  }
}
```

