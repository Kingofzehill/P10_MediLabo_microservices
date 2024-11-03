MédiLabo Microservices solution.
Projet 10 OpenClassRooms : Développez une solution en microservices pour votre client.
---
# Informations Générales
La solution de Microservices MédiLabo a pour objet d'administrer les données des Patients par le secrétariat et aux Praticiens de saisir les Notes concernant la patientèle. Elle inclut la génération de rapport de risque de Diabète pour un Patient.

La solution inclus les projets (microservices) suivants : 
- (PatientBackAPI) API permettant aux Secrétaires (Organizer) de gérer la base Patients de MédiLabo (ajout, modification, suppression, accès à un et à la liste des Patients).
- (PatientNoteBackAPI) API aux Practiens (Practitioner) leur permettant d'ajouter des Notes sur leurs Patients.
- (PatientDiabeteRiskBackAPI) API permettant d'évaluer le risque de Diabète d'un Patient à partir des termes déclencheurs détectées dans les Notes saisies par le Praticien.
- (PatientFront) Site web en Front permettant d'accéder aux méthodes des API selon les droits de l'utilisateur connecté en affichant les informations dans des interfaces utilisateurs.
- (OcelotAPIGateway) Une passerelle Ocelot permettant de router les requêtes entre les microserivces.
- Projet Docker Compose qui orchestre les microservices et définit le comportement de leur conteneur pour éxéctuer des applications à conteneurs multiples. 

# La solution inclus deux bases de données : 
- Une base de données relationnelle Microsoft SQL Server pour gérer les utilisateurs de l'applicatioin et héberger les données des Patients.
- Une base de données NoSQL MongoDB pour héberger les Notes des Praticiens.

# Technologies utilisées  
  - Solution Asp .Net Core sur framework .Net 8, développée en C# avec Microsoft Visual Studio Community 2022.
  - Microsoft Sql Server 2022 et Sql Server Management Studio 19.2.
  - MongoDB 8.0 et Mongosh 2.3.1.
  - Docker v2.1. 
  - Entity Framework Core v8.0.8 et .Net Core Identity.
  - Ocelot v23.3.3.
  - Swashbuckle Asp .Net Core Swagger v6.4.0. Documentation, description et génération de code APÏ (Application Programming Interface).
  - Json Web Token (JWT) Authentication v8.0.8. Pour échange sécurisé de jetons (tokens) garantissant intégrité et authenciité des données.  
  - Serilog v4.0.1. Gestion des Logs.
  - Asp .Net Core Healthcheck v8.0.1

# Prérequis    
Pour faire fonctionner le projet, vous devez au préalable avoir installé sur votre machine :
  - GitHub : https://git-scm.com/.
  - Docker Desktop : https://www.docker.com/. Les conteneurs Linux ont été utilisés.
  - Microsoft Sql Server 2022 : https://www.microsoft.com/fr-fr/sql-server/sql-server-2022.
  - Sql Server Management Studio 19.2 : https://learn.microsoft.com/fr-fr/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16.
  - MongoDB 8.0 et Mongosh 2.3.1. https://www.mongodb.com/fr-fr.

# Démarrer la solution
  1. Lancer Docker Desktop (en mode administrateur).
  2. Lancer Visual Studio (en mode administrateur).
  3. Dans Visual Studio, cloner le projet `https://github.com/Kingofzehill/P10_MediLabo_microservices.git`
  4. Veuiller adapter les ConnectionString des bases de données à votre configuration locale dans les fichiers appsettings.json des projets : PatientBackAPI, 
  5. Sélectionner le projet Docker Compose.
  6. Pour pouvoir exécuter la solution localement (localhost), vous devez installer un certificat de sécurité SSL. Il permet à l'application d'utiliser le protocole HTTPS pour les communications avec les différents microservices.
  Veuillez suivre la documentation Microsoft, créer un certificat auto-signé avec dotnet dev-certs : https://learn.microsoft.com/fr-fr/dotnet/core/additional-tools/self-signed-certificates-guide#create-a-self-signed-certificate 
  7. Nous vous conseillons d'activer l'utilisateur système sa dans SQL Server Management Studio (voir dossier Sécurités / Connexions). Dans les propriétés de la connexion sa, onglet Etat, passer "Autorisation de se connecter au moteur de base de données" à Autoriser, passer "Connexion" à Activé.
  8. La base de données Ms Sql Server P10_MediLabo_Patient-back est crée lorsque la solution est démarrée la première fois. Veuillez donc lancer la solution en debug une première fois et arrêter le débogage lorsque la page d'accueil de la solution est affichée.
  9. Vous pouvez ajouter les données de test Patients en suivant le document : 20241024_Create MsSQL database and patients test datas_v1.0.docx. Ce document se situe dans le répertoire ..\Ressources\Test Datas MsSqlServer\ des sources de la solution.  
  10. La base de données Mongo DB P10_MediLabo_PatientNotes-back est à créer (collection Notes) en suivant le document  : 20241024_Create MongoDB database and notes test datas_v1.docx.
  Ce document se situe dans le répertoire ..\Ressources\Test Data MongoDB\ des sources de la solution. Vous pouvez ajouter les données de test des Notes Patients en suivant ce document.
  11. Dans la console PowerShell Développeur de Visual Studio, saisir les commandes :
  `docker-compose build` : génère les images des conteneurs.
  `docker-compose up` : démarre les conteneurs.

  
# Utilisation
Si le démarrage de la solution a été effectué correctement, les conteneurs des services sont affichés dans Docker Desktop.
## Pour accéder au site Web du Front client : https://localhost:7288/
Dans le menu du site, cliquer sur le lien Se connecter et utiliser l'un des comptes utilisateurs ci-dessous (automatiquement créés lors du premier démarrage de la solution) pour vous connecter.

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

Quand vous êtes connectés pour accéder aux Patients, cliquer sur le lien Patients.

# Recommandations d'amélioration "Green"
- Utiliser un linter. Un Linter est un outil permettant d'analyser le code et l'améliorer. Il diagnostique et corrige des problèmes techniques, aide à maintenir le code lisible et cohérent. Mesure la qualité du code.
-- https://code.gouv.fr/fr/bluehats/ecocode/
-- https://fr.linkedin.com/advice/1/why-should-you-use-linter-your-code-skills-application-development-exyhf
- Refactoriser (réduire, recycler, réutiliser) le code pour réduire sa complexité et ainsi permettre qu'il soit plus performant, mais aussi réduire la quantité de code produit et le réutiliser pour moins consommer de ressources.
-- https://www.free-work.com/fr/tech-it/blog/actualites-informatiques/le-green-coding-vraie-tendance
- Implémenter un cache pour les APIs.
-- https://www.free-work.com/fr/tech-it/blog/actualites-informatiques/label-api-green-score

# Contact
## Mail : s.moureu.pro@gmail.com
## GitHub : https://github.com/Kingofzehill
