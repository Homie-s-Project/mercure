<!-- vscode-markdown-toc -->
- [Mercure](#mercure)
  - [Installation](#installation)
  - [Deployment](#deployment)
  - [API Docs Tests](#api-docs-tests)
  - [Pentest](#pentest)
  - [Test Credit Card](#test-credit-card)
  - [Nommage](#nommage)
    - [Branche](#branche)
  - [Routes](#routes)
  - [Image Docker](#image-docker)
  - [Environment Variables](#environment-variables)
    - [**Postgres**](#postgres)
    - [**pgAdmin4**](#pgadmin4)
    - [**mercure-api**](#mercure-api)
    - [**mercure-web**](#mercure-web)
    - [**mercure-grafana**](#mercure-grafana)
  - [ Connect Database](#-connect-database)
  - [Color Reference](#color-reference)
  - [Authors](#authors)

<!-- vscode-markdown-toc-config
	numbering=false
	autoSave=true
	/vscode-markdown-toc-config -->
<!-- /vscode-markdown-toc -->

# Mercure

![Docker Debug](https://github.com/Homie-s-Project/mercure/actions/workflows/docker-compose-debug.yml/badge.svg)
![Docker Production](https://github.com/Homie-s-Project/mercure/actions/workflows/docker-compose-prod.yml/badge.svg)
![Mercure API Testing](https://github.com/Homie-s-Project/mercure/actions/workflows/mercure-api-testing.yml/badge.svg)
![Mercure WEB Testing](https://github.com/Homie-s-Project/mercure/actions/workflows/mercure-web-testing.yml/badge.svg)
![Cleanup caches for closed branches](https://github.com/Homie-s-Project/mercure/actions/workflows/auto-clean-cache.yml/badge.svg)

Mercure la plateforme de e-commerce qui va vous changer votre façon de penser aux animaux.

Vous pouvez adopter ou même acheter des aliments pour votre meilleur ami.

Les labradors les plus choux de votre région sont sur notre plateforme, et vous attendent pour vivre une merveilleuse vie.

## <a name='Installation'></a>Installation

Se projet est fait pour fonctionner avec [Docker](https://www.docker.com).

Une fois docker installer est fonctionnel, il faudra clonner ce répository.

```bash
git clone https://github.com/Homie-s-Project/mercure.git
```

Une fois le répository clonner il faudra lancer docker pour qu'il installer tous les modules/packages nécessaires.

Avant de pouvoir lancer les commandes docker, il vous faut vous assurer que vous vous trouvez dans le ficher racines du projet.

Vous pouvez le lancer en mode debug (Mode de développement)

```bash
docker compose -f "docker-compose.debug.yml" up -d --build
```

## <a name='Deployment'></a>Deployment

Se projet est fait pour fonctionner avec [Docker](https://www.docker.com).

Une fois docker installer est fonctionnel, il faudra clonner ce répository.

```bash
git clone https://github.com/Homie-s-Project/mercure.git
```

Une fois le répository clonner il faudra lancer docker pour qu'il installer tous les modules/packages nécessaires.

Avant de pouvoir lancer les commandes docker, il vous faut vous assurer que vous vous trouvez dans le ficher racines du projet.

Vous pouvez le lancer en mode de production

```bash
docker compose -f "docker-compose.yml" up -d --build
```

## <a name='APIDocsTests'></a>API Docs Tests

Il vous faudra vous rendre sur ce fichier, où les explications y sont. --> [ICI](TEST_BACK.md) <--

## <a name='Pentest'></a>Pentest

Concernant les pentests, nous avons créé [un script en python](./pentest/pentest.py) qui permet d'extraire toute notre API et d'écrire un fichier de résultat avec le [résultat](./pentest/result.txt) de chaque rôle sur l'API.

Il écrit aussi un fichier markdown, qui permet l'affichage sur GitHub avec une version plus simple à lire. [Résultat Pentest](./pentest/RESULT_PENTEST.md).

Pour l'instant, le script n'arrive pas à tester toutes les requêtes qui nécessitent des paramètres. Mais en lisant le JSON de swagger il y a moyen de récupérer les paramètres et de pouvoir les simuler, ce qui pourrait être à faire dans le futur.

## <a name='TestCreditCard'></a>Test Credit Card

Selon la page de docs de Stripe, https://stripe.com/docs/testing#cards

Vous pouvez utiliser ces cartes pour tester leur service :

```
Numéro 	: 4242 4242 4242 4242
CCV 	: *3 chiffres aléatoires*
Date 	: *date après l'achat*
```

## <a name='Nommage'></a>Nommage

Toutes les variables du projet s'écrivent en anglais selon les conventions du langage.

### <a name='Branche'></a>Branche

Pour les branches, nous avons décidé de les nommer en anglais et ayant 1-3 mots pour décrire ce qui est fait. Les espaces sont remplacés par des "_".

Exemples:

- add_logs
- add_localisation
- edit_controller_auth

## <a name='Routes'></a>Routes

Les url et information comme les paramètres des routes sont disponible en consultant le fichier [ROUTES.md](ROUTES.md), les données ont été extraite grâce à [Widdershins](https://github.com/Mermade/widdershins).

## <a name='ImageDocker'></a>Image Docker

Notre projet utilise  [Docker](https://www.docker.com), pour des questions de simplicité et de temps. Grâce à Docker, nous pouvons transférer un environnement simplement et l'installer sur une autre machine grâce à une commande.

Nous utilisons **8** images dans notre projet dont 2 qui sont privé.

| Image Docker    | Description         |  URL |
| ------------------    | --------------- | ----------------|
| `mercure-api`*         | Cette image contient le code utile pour notre API. |[http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html) |
| `mercure-web`*        | Dans cette image, il se trouve tout notre code nécessaire pour avoir l'interface frontend avec Angular. | [http://localhost:4200/](http://localhost:4200/)
| [redis](https://hub.docker.com/_/redis)| Redis est l'image qui permet de gérer le cache de notre application.. | localhost:6379** |
| [postgres](https://hub.docker.com/_/postgres)| Postgres est l'image docker de notre base de données. | localhost:5432** |
| [dpage/pgadmin4](https://hub.docker.com/r/dpage/pgadmin4)  | Cette image permet d'avoir une interface d'utilisation de notre base de données.|  [http://localhost:5050/](http://localhost:5050/) |
| [grafana/grafana](https://hub.docker.com/r/grafana/grafana)  | Ce container permet d'avoir une page de dashboard de nos services.|  [http://localhost:30091/](http://localhost:30091/) |
| [ubuntu/prometheus](https://hub.docker.com/r/ubuntu/prometheus)  | Ce container permet de récupérer les informations d'un service.|  [http://localhost:30090/](http://localhost:30090/) |
| [wrouesnel/postgres_exporter](https://hub.docker.com/r/wrouesnel/postgres_exporter)  | Ce container permet de récupérer les informations de PostgreSQL et de le convertire pour l'utiliser avec Prometheus.|  [http://localhost:9187/](http://localhost:9187/) |

\* C'est des images non-publiques nous appartenant.

\**Ces URL ne peuvent pas être accédé depuis un navigateur web, ne sont pas utilisables à travers le protocole http.

*Les connexions pour la base de données, si elles n'ont pas été modifiées dans les variables d'environnement reste celle par défaut.*

## <a name='EnvironmentVariables'></a>Environment Variables

Pour exécuter ce projet, vous devrez ajouter les variables d'environnement suivantes à votre fichier .env\*

\*Le fichier avec les variables d'environnement est déjà prévu n'est pas encore utilisé par notre code.

### <a name='Postgres'></a>**Postgres**

| Environement name   | default         |
| ------------------- | --------------- |
| `POSTGRES_DB`       | mercure          |
| `POSTGRES_USER`     | mercure_user     |
| `POSTGRES_PASSWORD` | mercure_password |

### <a name='pgAdmin4'></a>**pgAdmin4**
*(Site local qui permet de gérer la postgres)*

| Environement name          | default     |
| -------------------------- | ----------- |
| `PGADMIN_DEFAULT_EMAIL`    | user@mercure.com |
| `PGADMIN_DEFAULT_PASSWORD` | mercure      |

### <a name='mercureApi'></a>**mercure-api**

| Environement name        | default       |
| ------------------------ | ------------- |
| `ASPNETCORE_ENVIRONMENT` | Development   |
| `ASPNETCORE_URLS`        | <http://+:5000> |

### <a name='mercureWeb'></a>**mercure-web**

| Environement name        | default       |
| ------------------------ | ------------- |
| `NODE_ENV` | development   |

### <a name='mercureGrafana'></a>**mercure-grafana**

| Environement name        | default       |
| ------------------------ | ------------- |
| `GF_SECURITY_ADMIN_USER` | mercure   |
| `GF_SECURITY_ADMIN_PASSWORD` | mercure_password   |


## <a nam='connectDatabase'></a> Connect Database

Pour se connecter à la base de données, vous pouvez utiliser ce logiciel [TablePlus](https://tableplus.com/download). Il est simple et facile d'utilisation.

Lorsque vous l'aurez téléchargé, vous pourrez créer une connexion en vous basant sur les informations de connexion se trouvant juste au-dessus dans le tablea **[Postgres](#postgres)**.

## <a name='ColorReference'></a>Color Reference

| Couleur             | Hex                                                                |
| ----------------- | ------------------------------------------------------------------ |
| Couleur avec propriété non définie* | ![#fff5cd](https://via.placeholder.com/15/fff5cd/fff5cd.png) #fff5cd |
| Couleur avec propriété non définie* | ![#6f3e2d](https://via.placeholder.com/15/6f3e2d/6f3e2d.png) #6f3e2d |
| Couleur avec propriété non définie* | ![#86965c](https://via.placeholder.com/15/86965c/86965c.png) #86965c |
| Couleur avec propriété non définie* | ![#5d6c3b](https://via.placeholder.com/15/5d6c3b/5d6c3b.png) #5d6c3b |
| Couleur avec propriété non définie* | ![#314d2d](https://via.placeholder.com/15/314d2d/314d2d.png) #314d2d |
| Couleur avec propriété non définie* | ![#1a2c16](https://via.placeholder.com/15/1a2c16/1a2c16.png) #1a2c16 |

\* Les couleurs n'ont pas une utilisation définient.

## <a name='Authors'></a>Authors

- [Christopher Andrade](https://github.com/Chriss052)
- [Romain Antunes](https://github.com/Flasssh)
- [Alexandre Botta](https://github.com/bottaalexandre)
- [William Pasquier](https://github.com/WilliamDevv)
- [Simão Romano Schindler](https://github.com/SchindlerSimao)
