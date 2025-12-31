FROM php:8.2-apache

# Installation des dépendances pour PostgreSQL et Symfony
RUN apt-get update && apt-get install -y \
    libpq-dev \
    libzip-dev \
    unzip \
    git \
    && docker-php-ext-install pdo pdo_pgsql zip

# Activation de la réécriture d'URL pour Symfony
RUN a2enmod rewrite

# Installation de Composer
COPY --from=composer:latest /usr/bin/composer /usr/bin/composer

WORKDIR /var/www/html

# Copie du projet
COPY . /var/www/html

# Installation des dépendances sans les scripts de dev
RUN composer install --no-dev --optimize-autoloader --no-interaction

# Droits d'écriture pour le cache et les logs
RUN chown -R www-data:www-data /var/www/html/var /var/www/html/public

# Configuration Apache : Pointer vers le dossier /public
RUN sed -i 's|/var/www/html|/var/www/html/public|g' /etc/apache2/sites-available/000-default.conf

# Render utilise souvent le port 10000 ou 80 par défaut. 
# Si tu forces 10000 ici, assure-toi de configurer le port dans le Dashboard Render.
EXPOSE 10000

# Script pour changer le port d'écoute d'Apache dynamiquement si nécessaire
RUN sed -i 's/80/10000/g' /etc/apache2/ports.conf /etc/apache2/sites-available/000-default.conf

CMD ["apache2-foreground"]