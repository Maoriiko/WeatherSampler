# 🌤 WeatherSampler

פרויקט **WeatherSampler** נועד לדגום את מזג האוויר בעיר שבחרתי לטייל בה בקרוב, לשלוח את הנתונים ל-RabbitMQ, ולאחר מכן להעביר אותם ל-Elasticsearch דרך Logstash. בנוסף, יצרתי דאשבורד ב-Grafana שמציג את הנתונים בזמן אמת עם התראות חכמות על הטמפרטורה. כל התשתית מורצת באמצעות Docker Compose.

---

## 🚀 מטרות הפרויקט

1. דגימה אוטומטית של מזג האוויר מדי שעה.
2. שליחת הנתונים ל-RabbitMQ לניטור.
3. שימוש ב-Logstash להעברת הנתונים ל-Elasticsearch.
4. הצגת הנתונים ב-Grafana עם התראות:
   - 🔴 חום מעל 24°C
   - ❄️ חום נמוך מ-0°C
5. בניית Pipeline ב-GitLab CI/CD עם שלבים מקבילים:
   - clone → build → unittest → deploy → הודעה ל-Grafana.
6. (נקודת בונוס) שימוש ב-webhook להפעלת pipeline בעת git push.

---

## 🛠️ טכנולוגיות וכלים

- **שפת תכנות:** C# (.NET 8)
- **מסדי נתונים:** Elasticsearch
- **תורים:** RabbitMQ
- **איסוף נתונים:** Logstash
- **דאשבורד:** Grafana
- **CI/CD:** GitLab Pipelines
- **קונטיינרים:** Docker & Docker Compose
- **API:** OpenWeatherMap REST API

---

## 📦 מבנה התיקייה (נקי)

```
WeatherSampler/
│   .dockerignore
│   .gitignore
│   docker-compose.dcproj
│   docker-compose.override.yml
│   docker-compose.yml
│   launchSettings.json
│   README.md
│   WeatherSampler.sln
│
├───grafana
│   ├───dashboards
│   │       weather_dashboard.json
│   │
│   └───provisioning
│       ├───dashboards
│       │       dashboard.yml
│       │
│       └───datasources
│               datasource.yml
│
├───logstash
│       pipeline.conf
│
└───WeatherSampler
    │   .dockerignore
    │   appsettings.json
    │   Dockerfile
    │   gitlab-ci.yml
    │   OpenWeatherSettings.cs
    │   Program.cs
    │   RabbitMqPublisher.cs
    │   RabbitMqSettings.cs
    │   SamplingSettings.cs
    │   SimpleWeatherMessage.cs
    │   WeatherClient.cs
    │   WeatherSampler.csproj
    │   WeatherSampler.csproj.user
    │   Worker.cs
    │
    ├───Models
    │       OpenWeatherResponse.cs
    │       WeatherMessage.cs
    │
    └───Properties
            launchSettings.json
```

---

## 🔗 Flow של הנתונים

1. **WeatherSampler** → דגימת מזג האוויר
2. שליחת הנתונים ל-**RabbitMQ**
3. **Logstash** לוקח את הנתונים ומעביר ל-**Elasticsearch**
4. **Grafana** מציגה את הנתונים ודואגת להתראות חכמות

---

## 📝 Pipeline ב-GitLab

שלבים מקבילים ומפורטות:

- **Clone:** הורדת הקוד מה-Repository  
- **Build:** בניית האפליקציה עם .NET 8  
- **UnitTest:** הרצת בדיקות יחידה  
- **Deploy:** בניית אימג' Docker, דחיפה ל-DockerHub והרצת הקונטיינרים  
- **Notification:** שליחת הודעה ל-Grafana בסיום Pipeline  

---

## ⚠️ אתגרים והלבטים

### אתגרים:
- אין ידע מוקדם ב-Elasticsearch, RabbitMQ, Logstash → נדרשתי ללמוד ולהטמיע את הכל.
- הבנה איך להבטיח דיוק של זמן דגימה ברמת מילי-שנייה.
- חיבור כל המערכות יחד תוך שימוש ב-Docker Compose.

### ספקות:
- בתחילה לא ידעתי האם כדאי לוותר על חלק מהמערכות, כי האפליקציה עבדה אבל המערכות לא “דיברו” ביניהן.
- בסופו של דבר למדתי יותר והצלחתי להשתמש בכלים בצורה יעילה, ולהתקדם לפתרון מלא.

---

## 🏆 תיעוד אישי

למדתי רבות על עולם ה-Messaging (RabbitMQ), Elastic Stack, ו-Visualizing Data ב-Grafana. הפרויקט לימד אותי גם איך לנהל Pipeline ב-GitLab CI/CD עם שלבים מקבילים ושליחת התראות.

---

## 🌐 קישורי עניין

- [OpenWeatherMap API](https://openweathermap.org/api)
- [RabbitMQ](https://www.rabbitmq.com/)
- [Elastic Stack](https://www.elastic.co/elastic-stack/)
- [Grafana](https://grafana.com/)

---

# 🌤 WeatherSampler (English)

**WeatherSampler** is designed to sample weather data from the city I plan to visit soon, send it to RabbitMQ, and then push it to Elasticsearch via Logstash. A Grafana dashboard visualizes the data with smart alerts. All infrastructure runs using Docker Compose.

## 🚀 Project Goals

1. Automatic weather sampling every hour.
2. Send data to RabbitMQ for monitoring.
3. Use Logstash to transfer data to Elasticsearch.
4. Grafana dashboard with alerts:
   - 🔴 Temp above 24°C
   - ❄️ Temp below 0°C
5. GitLab CI/CD Pipeline with parallel stages:
   - clone → build → unittest → deploy → notification to Grafana
6. (Bonus) Use webhook to trigger pipeline on git push.

## 🛠️ Technologies

- **Language:** C# (.NET 8)
- **Database:** Elasticsearch
- **Queue:** RabbitMQ
- **Data Collector:** Logstash
- **Dashboard:** Grafana
- **CI/CD:** GitLab Pipelines
- **Containers:** Docker & Docker Compose
- **API:** OpenWeatherMap REST API

## ⚠️ Challenges & Considerations

**Challenges:**
- No prior knowledge of Elasticsearch, RabbitMQ, Logstash → had to learn and implement.
- Ensuring millisecond-level accuracy for weather sampling.
- Connecting all services using Docker Compose.

**Considerations:**
- Initially unsure if I should simplify, as the app worked but services were not fully communicating.
- Eventually, I researched and used the proper tools to achieve full functionality.

## 🏆 Personal Notes

This project taught me a lot about messaging systems, the Elastic Stack, data visualization in Grafana, and building CI/CD pipelines with parallel stages and notifications.