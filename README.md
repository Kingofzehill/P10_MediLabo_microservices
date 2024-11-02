MédiLabo Microservices solution.
Projet 10 OpenClassRooms : Développez une solution en microservices pour votre client.
---
# Informations Générales
La solution de Microservices MédiLabo a pour objet de permettre d'administrer les données des Patients par le secrétariat et aux Praticiens de saisir les Notes concernant la patientèle. 
Elle inclut la génération de rapport de risque de Diabète pour un Patient.
Ce projet inclus les microservices suivants : 
- API (PatientBackAPI) permettant aux Secrétaires (Organizer) de gérer la base Patients de MédiLabo (ajout, modification, suppression, accès à un et à la liste des Patients).
- API (PatientNoteBackAPI) aux Practiens (Practitioner) leur permettant d'ajouter des Notes sur leurs Patients.
- API (PatientDiabeteRiskBackAPI) permettant d'évaluer le risque de Diabète d'un Patient à partir des termes déclencheurs détectées dans les Notes saisies par le Praticien.
- Un site web en Front (PatientFront) permettant d'accéder aux méthodes des API selon les droits de l'utilisateur connecté en affichant les informations dans des interfaces utilisateurs.
- Une passerelle (OcelotAPIGateway) Ocelot permettant de router les requêtes entre les microserivces.
- Un projet Docker Compose qui orchestre les microservices et définit le comportement de leur conteneur pour éxéctuer des applications à conteneurs multiples. 

# Ce projet inclus deux bases de données : 
- Une base de données relationnelle Microsoft SQL Server pour gérer les utilisateurs de l'applicatioin et héberger les données des Patients.
- Une base de données NoSQL MongoDB pour héberger les Notes des Praticiens.

# Technologies utilisées  
  - Solution Asp .Net Core développée avec le framework .Net 8 en C# avec Microsoft Visual Studio Community 2022.
  - Microsoft Sql Server 2022 et Sql Server Management Studio 19.2.
  - MongoDB 8.0 et Mongosh 2.3.1.
  - RxJS
  - Docker v2.1. Ce projet utilise les conteneurs Linux.
  - Entity Framework Core
  - Swashbuckle Asp .Net Core Swagger v6.4.0. Documentation, description et génération de code APÏ (Application Programming Interface).
  - Json Web Token (JWT) Authentication v8.0.8. Pour échange sécurisé de jetons (tokens) garantissant intégrité et authenciité des données.  
  - Entity Framework Core v8.0.8 et .Net Core Identity.
  - Serilog v4.0.1. Gestion des Logs.
  - Ocelot v23.3.3.
  - Asp .Net Core Healthcheck v8.0.1

# Prérequis    
Pour faire fonctionner le projet, vous devez au préalable avoir installé sur votre machine :
  - GitHub : https://git-scm.com/.
  - Docker Desktop : https://www.docker.com/.
  - Microsoft Sql Server 2022 : https://www.microsoft.com/fr-fr/sql-server/sql-server-2022.
  - Sql Server Management Studio 19.2 : https://learn.microsoft.com/fr-fr/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16.
  - MongoDB 8.0 et Mongosh 2.3.1. https://www.mongodb.com/fr-fr.

# Démarrer la solution
  1. Lancer Docker Desktop (en mode administrateur).
  2. Ouvrir Visual Studio (en mode administrateur).
  3. Cloner le projet `https://github.com/Kingofzehill/P10_MediLabo_microservices.git`
  4. Sélectionner le projet Docker Compose.
  5. Dans la console PowerShell Développeur de Visual Studio, saisir les commandes :
  `docker-compose build` : génère les images des conteneurs.
  `docker-compose up` : démarre les conteneurs.
  
# Utilisation
Si le démarrage de la solution a été effectué correctement, les conteneurs des services sont affichés et tournent dans Docker Desktop.
#### Pour accéder au site Web du Front client : https://localhost:7288/
Dans le menu du site, cliquer sur le lien Se connecter et utiliser l'un des comptes utilisateurs suivants (qui ont été automatiquement créés).

## __Compte de test Secrétériat__:
- Utilisateur : Organizer
- Mot de passe : Org202478*
## Rôle Organizer.
Ce rôle permet d'administrer les Patients (création, modification, suppression) et d'accéder à un ou à la liste des Patients dans le Menu Patients. 
## __Compte de test Praticien__:
- Utilisateur : Practitioner
- Mot de passe : Pra202478*
## Rôle Practitioner.
Ce rôle permet au Praticien de consulter la liste des Patients et un Patient. De gérer ses notes (création et suppression) et d'en afficher la liste pour un Patient. 
Ce rôle permet aussi de générer un rapport de risque de diabète pour un Patient.
## __Compte Administrateur__:
- Utilisateur : Admin
- Mot de passe : Admin202478*
## Rôles Organizer et Practitioner.

Pour accéder aux Patients, cliquer sur le lien Patients.

# Recommandations d'amélioration "Green"
- Implémenter un cache pour les APIs.
-- https://www.free-work.com/fr/tech-it/blog/actualites-informatiques/label-api-green-score
- Refactoriser (réduire, recycler, réutiliser) le code pour réduire sa complexité et ainsi permettre qu'il soit plus performant, mais aussi réduire la quantité de code produit et le réutiliser pour moins consommer de ressources.
-- https://www.free-work.com/fr/tech-it/blog/actualites-informatiques/le-green-coding-vraie-tendance
- Utiliser un linter. Un Linter est un outil permettant d'analyser le code et l'améliorer. Il diagnostique et corrige des problèmes techniques, aide à maintenir le code lisible et cohérent. Mesure la qualité du code.
-- https://code.gouv.fr/fr/bluehats/ecocode/
-- https://fr.linkedin.com/advice/1/why-should-you-use-linter-your-code-skills-application-development-exyhf

# Contact
## Mail : s.moureu.pro@gmail.com
## GitHub : https://github.com/Kingofzehill
