// Brasil Burger - JavaScript

// Fonction pour mettre à jour le compteur du panier
function updatePanierCount() {
    fetch('/Catalogue/GetPanierCount')
        .then(response => response.json())
        .then(data => {
            const badge = document.getElementById('panier-count');
            if (badge) {
                badge.textContent = data.count;
                if (data.count > 0) {
                    badge.style.display = 'inline-block';
                } else {
                    badge.style.display = 'none';
                }
            }
        })
        .catch(error => console.error('Erreur:', error));
}

// Mettre à jour le compteur au chargement de la page
document.addEventListener('DOMContentLoaded', function() {
    updatePanierCount();
    
    // Auto-dismiss alerts après 5 secondes
    const alerts = document.querySelectorAll('.alert');
    alerts.forEach(alert => {
        setTimeout(() => {
            const bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        }, 5000);
    });
});

// Fonction pour confirmer la suppression
function confirmerSuppression(message) {
    return confirm(message || 'Êtes-vous sûr de vouloir supprimer cet élément ?');
}

// Fonction pour formater les prix
function formatPrice(price) {
    return new Intl.NumberFormat('fr-FR').format(price) + ' FCFA';
}

// Gestion du filtre catalogue (optionnel - peut être fait côté serveur)
function filtrer(type) {
    const burgers = document.querySelectorAll('.burger-card');
    const menus = document.querySelectorAll('.menu-card');
    
    if (type === 'tous') {
        burgers.forEach(b => b.style.display = 'block');
        menus.forEach(m => m.style.display = 'block');
    } else if (type === 'burgers') {
        burgers.forEach(b => b.style.display = 'block');
        menus.forEach(m => m.style.display = 'none');
    } else if (type === 'menus') {
        burgers.forEach(b => b.style.display = 'none');
        menus.forEach(m => m.style.display = 'block');
    }
}

// Validation formulaire inscription
function validateInscription(form) {
    const password = form.querySelector('input[name="MotDePasse"]').value;
    const confirmPassword = form.querySelector('input[name="ConfirmationMotDePasse"]').value;
    
    if (password !== confirmPassword) {
        alert('Les mots de passe ne correspondent pas !');
        return false;
    }
    
    if (password.length < 6) {
        alert('Le mot de passe doit contenir au moins 6 caractères !');
        return false;
    }
    
    return true;
}

// Smooth scroll
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            target.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });
        }
    });
});

// Calcul dynamique du total dans le panier
function calculerTotal() {
    let total = 0;
    document.querySelectorAll('.item-panier').forEach(item => {
        const prix = parseFloat(item.dataset.prix);
        const quantite = parseInt(item.querySelector('.quantite').value);
        total += prix * quantite;
    });
    
    const totalElement = document.getElementById('panier-total');
    if (totalElement) {
        totalElement.textContent = formatPrice(total);
    }
}

// Confirmation avant de vider le panier
function viderPanier() {
    if (confirm('Voulez-vous vraiment vider votre panier ?')) {
        window.location.href = '/Commande/ViderPanier';
    }
}

console.log('🍔 Brasil Burger - JS chargé avec succès !');
