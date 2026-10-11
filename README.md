## API Endpoints

| Method | Endpoint | Description | Success Code | Failure Codes |
| :--- | :--- | :--- | :--- | :--- |
| `GET` | `/api/elections` | Retrieves a list of active elections | `200 OK` | `500 Internal Server Error` |
| `GET` | `/api/elections/{id}` | Retrieves details for a specific election | `200 OK` | `404 Not Found`, `500 Internal Server Error` |
| `GET` | `/api/candidates` | Retrieves candidate profiles matching filter criteria | `200 OK` | `400 Bad Request`, `500 Internal Server Error` |
| `GET` | `/api/candidates/{id}` | Retrieves detailed candidate profile and platform data | `200 OK` | `404 Not Found`, `500 Internal Server Error` |
| `POST` | `/api/candidates` | Registers a new candidate profile | `201 Created` | `400 Bad Request`, `409 Conflict`, `500 Internal Server Error` |
| `PUT` | `/api/candidates/{id}` | Updates existing candidate information | `204 No Content` | `400 Bad Request`, `404 Not Found`, `500 Internal Server Error` |
| `DELETE` | `/api/candidates/{id}` | Removes a candidate record | `204 No Content` | `404 Not Found`, `500 Internal Server Error` |
| `GET` | `/api/ballots` | Retrieves ballot items based on jurisdiction or address | `200 OK` | `400 Bad Request`, `500 Internal Server Error` |
