# Mercure

![Docker Debug](https://github.com/Homie-s-Project/mercure/actions/workflows/docker-compose-debug.yml/badge.svg)
![Docker Production](https://github.com/Homie-s-Project/mercure/actions/workflows/docker-compose-prod.yml/badge.svg)
![Mercure API Testing](https://github.com/Homie-s-Project/mercure/actions/workflows/mercure-api-testing.yml/badge.svg)
![Mercure WEB Testing](https://github.com/Homie-s-Project/mercure/actions/workflows/mercure-web-testing.yml/badge.svg)



Mercure la plateforme de e-commerce qui va vous changer votre façon de penser aux animaux. 

Vous pouvez adopter ou même acheter des aliments pour votre meilleur ami. 


Les labradors les plus choux de votre région sont sur notre plateforme, et vous attendent pour vivre une merveilleuse vie.

## Installation

Se projet est fait pour fonctionner avec [Docker](https://www.docker.com).

Une fois docker installer est fonctionnel, il faudra clonner ce répository.

```bash
$ git clone https://github.com/Homie-s-Project/mercure.git
```

Une fois le répository clonner il faudra lancer docker pour qu'il installer tous les modules/packages nécessaires.

Avant de pouvoir lancer les commandes docker, il vous faut vous assurer que vous vous trouvez dans le ficher racines du projet.

Vous pouvez le lancer en mode debug (Mode de développement)

```bash
$ docker compose -f "docker-compose.debug.yml" up -d --build
```

## Deployment

Se projet est fait pour fonctionner avec [Docker](https://www.docker.com).

Une fois docker installer est fonctionnel, il faudra clonner ce répository.

```bash
$ git clone https://github.com/Homie-s-Project/mercure.git
```

Une fois le répository clonner il faudra lancer docker pour qu'il installer tous les modules/packages nécessaires.

Avant de pouvoir lancer les commandes docker, il vous faut vous assurer que vous vous trouvez dans le ficher racines du projet.

Vous pouvez le lancer en mode de production

```bash
$ docker compose -f "docker-compose.yml" up -d --build
```

## Nommage
Toutes les variables du projet s'écrivent en anglais selon les conventions du langage.

### Branche

Pour les branches, nous avons décidé de les nommer en anglais et ayant 1-3 mots pour décrire ce qui est fait. Les espaces sont remplacés par des "_".

Exemples:
  * add_logs
  * add_localisation
  * edit_controller_auth

## Image Docker
Notre projet utilise  [Docker](https://www.docker.com), pour des questions de simplicité et de temps. Grâce à Docker, nous pouvons transférer un environnement simplement et l'installer sur une autre machine grâce à une commande.

Nous utilisons **4** images dans notre projet avec chacune des images une particularité.

| Image Docker   	| Description         |  URL |
| ------------------    | --------------- | ----------------|
| `mercure-api`*         | Cette image contient le code utile pour notre API. |[http://localhost:5001/swagger/index.html](http://localhost:5001/swagger/index.html) |
| `mercure-web`*     	  | Dans cette image, il se trouve tout notre code nécessaire pour avoir l'interface frontend avec Angular. | [http://localhost:4200/](http://localhost:4200/)
| [postgres](https://hub.docker.com/_/postgres)| Postgres est l'image docker de notre base de données. | -** |
| [dpage/pgadmin4](https://hub.docker.com/r/dpage/pgadmin4) 	| Cette image permet d'avoir une interface d'utilisation de notre base de données.|  [http://localhost:5050/](http://localhost:5050/) |

\* C'est des images non-publiques nous appartenant.

\**Aucun lien URL pour notre base de données.

*Les connexions pour la base de données, si elles n'ont pas été modifiées dans les variables d'environnement reste celle par défaut.*

## Environment Variables

Pour exécuter ce projet, vous devrez ajouter les variables d'environnement suivantes à votre fichier .env\*

\*Le fichier avec les variables d'environnement est déjà prévu n'est pas encore utilisé par notre code.

**Postgres**

| Environement name   | default         |
| ------------------- | --------------- |
| `POSTGRES_DB`       | mercure          |
| `POSTGRES_USER`     | mercure_user     |
| `POSTGRES_PASSWORD` | mercure_password |

**pgAdmin4** *(Site local qui permet de gérer la postgres)*

| Environement name          | default     |
| -------------------------- | ----------- |
| `PGADMIN_DEFAULT_EMAIL`    | user@mercure.com |
| `PGADMIN_DEFAULT_PASSWORD` | mercure      |

**mercure-api**

| Environement name        | default       |
| ------------------------ | ------------- |
| `ASPNETCORE_ENVIRONMENT` | Development   |
| `ASPNETCORE_URLS`        | http://+:5001 |

**mercure-web**

| Environement name        | default       |
| ------------------------ | ------------- |
| `NODE_ENV` | development   |

## Color Reference

| Couleur             | Hex                                                                |
| ----------------- | ------------------------------------------------------------------ |
| Couleur avec propriété non définie* | ![#fff5cd](https://via.placeholder.com/15/fff5cd/fff5cd.png) #fff5cd |
| Couleur avec propriété non définie* | ![#6f3e2d](https://via.placeholder.com/15/6f3e2d/6f3e2d.png) #6f3e2d |
| Couleur avec propriété non définie* | ![#86965c](https://via.placeholder.com/15/86965c/86965c.png) #86965c |
| Couleur avec propriété non définie* | ![#5d6c3b](https://via.placeholder.com/15/5d6c3b/5d6c3b.png) #5d6c3b |
| Couleur avec propriété non définie* | ![#314d2d](https://via.placeholder.com/15/314d2d/314d2d.png) #314d2d |
| Couleur avec propriété non définie* | ![#1a2c16](https://via.placeholder.com/15/1a2c16/1a2c16.png) #1a2c16 |

\* Les couleurs n'ont pas une utilisation précise définie.

## Authors

-   [Christopher Andrade](https://github.com/Chriss052)
-   [Romain Antunes](https://github.com/Flasssh)
-   [Alexandre Botta](https://github.com/bottaalexandre)
-   [William Pasquier](https://github.com/WilliamDevv)
