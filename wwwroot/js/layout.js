/**
 * SAED Layout Enhancement Script
 * Simplified version for better Bootstrap compatibility
 */
(function() {
    'use strict';

    // DOM elements
    let backToTopButton = null;

    /**
     * Initialize layout functionality
     */
    function initLayout() {
        // Cache DOM elements
        backToTopButton = document.getElementById('backToTop');

        // Initialize features
        initBackToTop();
        initEmailTruncation();
        
        console.log('SAED Layout initialized');
    }

    /**
     * Initialize email truncation for user display
     */
    function initEmailTruncation() {
        const userEmailElements = document.querySelectorAll('.user-email-display');
        
        userEmailElements.forEach(element => {
            const text = element.textContent.trim();
            
            // Store original email for reference
            if (!element.dataset.originalEmail) {
                element.dataset.originalEmail = text;
            }
            
            // Check if it's an email (contains @)
            if (text.includes('@')) {
                updateEmailDisplay(element);
            }
        });

        // Update on window resize
        let resizeTimeout;
        window.addEventListener('resize', () => {
            clearTimeout(resizeTimeout);
            resizeTimeout = setTimeout(() => {
                userEmailElements.forEach(element => {
                    if (element.dataset.originalEmail && element.dataset.originalEmail.includes('@')) {
                        updateEmailDisplay(element);
                    }
                });
            }, 300);
        });
    }

    /**
     * Update email display based on current screen size
     */
    function updateEmailDisplay(element) {
        const originalEmail = element.dataset.originalEmail;
        if (!originalEmail) return;

        // Don't truncate on mobile
        if (window.innerWidth <= 992) {
            element.textContent = originalEmail;
            element.classList.remove('user-email-truncated');
            element.removeAttribute('title');
            return;
        }

        // Determine max length based on screen size
        let maxLength = 20; // Default
        if (window.innerWidth <= 1200) {
            maxLength = 16; // Smaller screens
        }

        const truncatedEmail = truncateEmail(originalEmail, maxLength);
        if (truncatedEmail !== originalEmail) {
            element.textContent = truncatedEmail;
            element.classList.add('user-email-truncated');
            element.setAttribute('title', originalEmail); // Show full email on hover
        } else {
            element.textContent = originalEmail;
            element.classList.remove('user-email-truncated');
            element.removeAttribute('title');
        }
    }

    /**
     * Truncate email to format: username...@domain.com
     * @param {string} email - The email to truncate
     * @param {number} maxLength - Maximum length before truncating
     * @returns {string} - Truncated email
     */
    function truncateEmail(email, maxLength = 20) {
        if (!email || !email.includes('@') || email.length <= maxLength) {
            return email;
        }

        const [localPart, domain] = email.split('@');
        const domainPart = '@' + domain;
        
        // If domain part is too long, just use ellipsis at the end
        if (domainPart.length >= maxLength - 3) {
            return email.substring(0, maxLength - 3) + '...';
        }

        // Calculate how much space we have for the local part
        const availableSpace = maxLength - domainPart.length - 3; // 3 for "..."
        
        if (availableSpace <= 1) {
            return email.substring(0, maxLength - 3) + '...';
        }

        // Truncate local part and add ellipsis before @
        const truncatedLocal = localPart.substring(0, availableSpace);
        return truncatedLocal + '...' + domainPart;
    }

    /**
     * Initialize back to top functionality
     */
    function initBackToTop() {
        if (!backToTopButton) return;

        // Show/hide button based on scroll position
        const toggleButton = () => {
            if (window.pageYOffset > 300) {
                backToTopButton.classList.add('show');
            } else {
                backToTopButton.classList.remove('show');
            }
        };

        // Smooth scroll to top
        backToTopButton.addEventListener('click', function(e) {
            e.preventDefault();
            window.scrollTo({
                top: 0,
                behavior: 'smooth'
            });
        });

        // Listen to scroll events with throttling
        let ticking = false;
        window.addEventListener('scroll', function() {
            if (!ticking) {
                requestAnimationFrame(function() {
                    toggleButton();
                    ticking = false;
                });
                ticking = true;
            }
        });

        // Initial check
        toggleButton();
    }

    /**
     * Smart dropdown positioning to prevent overflow
     */
    function initSmartDropdowns() {
        const dropdowns = document.querySelectorAll('.navbar-nav .dropdown');
        
        dropdowns.forEach(dropdown => {
            const toggle = dropdown.querySelector('.dropdown-toggle');
            const menu = dropdown.querySelector('.dropdown-menu');
            
            if (!toggle || !menu) return;
            
            // Listen for dropdown show events
            toggle.addEventListener('click', () => {
                setTimeout(() => {
                    if (menu.classList.contains('show')) {
                        adjustDropdownPosition(dropdown, menu);
                    }
                }, 10);
            });
            
            // Also check on hover for desktop
            if (window.innerWidth > 992) {
                dropdown.addEventListener('mouseenter', () => {
                    setTimeout(() => {
                        if (menu.classList.contains('show')) {
                            adjustDropdownPosition(dropdown, menu);
                        }
                    }, 10);
                });
            }
        });
    }
    
    /**
     * Adjust dropdown position if it would overflow
     */
    function adjustDropdownPosition(dropdown, menu) {
        // Only adjust on desktop
        if (window.innerWidth <= 992) return;
        
        const rect = menu.getBoundingClientRect();
        const viewportWidth = window.innerWidth;
        const dropdownRect = dropdown.getBoundingClientRect();
        const menuWidth = rect.width;
        
        // Reset positioning classes
        menu.classList.remove('dropdown-menu-end');
        menu.style.left = '';
        menu.style.right = '';
        menu.style.transform = '';
        
        // Calculate if dropdown would overflow on the right
        const rightOverflow = (dropdownRect.left + menuWidth) > (viewportWidth - 20);
        
        // Calculate if dropdown would overflow on the left
        const leftOverflow = (dropdownRect.right - menuWidth) < 20;
        
        if (rightOverflow && !leftOverflow) {
            // Position dropdown to the right (align right edge with toggle)
            menu.style.left = 'auto';
            menu.style.right = '0';
        } else if (leftOverflow && !rightOverflow) {
            // Position dropdown to the left
            menu.style.left = '0';
            menu.style.right = 'auto';
        } else {
            // Default positioning (left aligned)
            menu.style.left = '0';
            menu.style.right = 'auto';
        }
    }

    // Initialize when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', () => {
            initLayout();
            initSmartDropdowns();
        });
    } else {
        initLayout();
        initSmartDropdowns();
    }

})();