document.addEventListener('DOMContentLoaded', function() {
    // Subscribe form handling
    const subscribeForm = document.querySelector('.input-group');
    const subscribeEmail = document.getElementById('subscribeEmail');
    const subscribeButton = document.querySelector('.btn-success');

    if (subscribeButton) {
        subscribeButton.addEventListener('click', function() {
            const email = subscribeEmail.value.trim();
            
            if (!isValidEmail(email)) {
                showToast('Please enter a valid email address');
                return;
            }

            // Simulate subscription (replace with actual API call)
            showToast('Thank you for subscribing!');
            subscribeEmail.value = '';
        });
    }

    // Email validation helper
    function isValidEmail(email) {
        const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return re.test(email);
    }

    // Simple toast notification
    function showToast(message) {
        const toast = document.createElement('div');
        toast.className = 'toast-notification';
        toast.textContent = message;
        
        document.body.appendChild(toast);
        
        setTimeout(() => {
            toast.classList.add('show');
        }, 100);

        setTimeout(() => {
            toast.classList.remove('show');
            setTimeout(() => {
                document.body.removeChild(toast);
            }, 300);
        }, 3000);
    }

    // Smooth scroll for footer links
    const footerLinks = document.querySelectorAll('.footer-link-list a');
    
    footerLinks.forEach(link => {
        link.addEventListener('click', function(e) {
            const href = this.getAttribute('href');
            
            if (href.startsWith('#')) {
                e.preventDefault();
                const target = document.querySelector(href);
                if (target) {
                    target.scrollIntoView({
                        behavior: 'smooth'
                    });
                }
            }
        });
    });
});

// Add toast notification styles
const style = document.createElement('style');
style.textContent = `
    .toast-notification {
        position: fixed;
        bottom: 20px;
        right: 20px;
        background: #333;
        color: white;
        padding: 15px 25px;
        border-radius: 5px;
        opacity: 0;
        transform: translateY(100%);
        transition: all 0.3s ease;
        z-index: 1000;
    }

    .toast-notification.show {
        opacity: 1;
        transform: translateY(0);
    }
`;
document.head.appendChild(style); 

    document.addEventListener('DOMContentLoaded', function () {
    // Subscribe form handling
    const subscribeForm = document.querySelector('.input-group');
    const subscribeEmail = document.getElementById('subscribeEmail');
    const subscribeButton = document.querySelector('.btn-success');

    if (subscribeButton) {
        subscribeButton.addEventListener('click', function() {
            const email = subscribeEmail.value.trim();
            
            if (!isValidEmail(email)) {
                showToast('Please enter a valid email address');
                return;
            }

            // Simulate subscription (replace with actual API call)
            showToast('Thank you for subscribing!');
            subscribeEmail.value = '';
        });
    }
    // Simple toast notification
    function showToast(message) {
        const toast = document.createElement('div');
        toast.className = 'toast-notification';
        toast.textContent = message;
        
        document.body.appendChild(toast);
        
        setTimeout(() => {
            toast.classList.add('show');
        }, 100);

        setTimeout(() => {
            toast.classList.remove('show');
            setTimeout(() => {
                document.body.removeChild(toast);
            }, 300);
        }, 3000);
    }
    // Smooth scroll for footer links
    const footerLinks = document.querySelectorAll('.footer-link-list a');
    
    footerLinks.forEach(link => {
        link.addEventListener('click', function(e) {
            const href = this.getAttribute('href');
            
            if (href.startsWith('#')) {
                e.preventDefault();
                const target = document.querySelector(href);
                if (target) {
                    target.scrollIntoView({
                        behavior: 'smooth'
                    });
                }
            }
        });
    });
});

// Add toast notification styles
const style = document.createElement('style');
style.textContent = `
    .toast-notification {
        position: fixed;
        bottom: 20px;
        right: 20px;
        background: #333;
        color: white;
        padding: 15px 25px;
        border-radius: 5px;
        opacity: 0;
        transform: translateY(100%);
        transition: all 0.3s ease;
        z-index: 1000;
    }

    .toast-notification.show {
        opacity: 1;
        transform: translateY(0);
    }
`;
document.head.appendChild(style);