/**
 * SAED Homepage Interactive Features
 * Modern JavaScript for enhanced user experience
 */

(function() {
    'use strict';

    // DOM loaded state
    let domReady = false;
    
    // Initialize when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }

    function init() {
        domReady = true;
        
        // Initialize all features
        initScrollAnimations();
        initCounterAnimations();
        initParallaxEffects();
        initCardHoverEffects();
        initSmoothScrolling();
        
        console.log('🚀 SAED Homepage initialized successfully');
    }

    /**
     * Scroll-triggered animations using Intersection Observer
     */
    function initScrollAnimations() {
        const animatedElements = document.querySelectorAll('.animate-on-scroll');
        
        if (!animatedElements.length) return;

        const observerOptions = {
            threshold: 0.1,
            rootMargin: '0px 0px -50px 0px'
        };

        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('in-view');
                    // Optional: Stop observing once animated
                    observer.unobserve(entry.target);
                }
            });
        }, observerOptions);

        animatedElements.forEach(el => observer.observe(el));
    }

    /**
     * Animated counter for statistics
     */
    function initCounterAnimations() {
        const counters = document.querySelectorAll('.stat-number');
        
        if (!counters.length) return;

        const observerOptions = {
            threshold: 0.5
        };

        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    animateCounter(entry.target);
                    observer.unobserve(entry.target);
                }
            });
        }, observerOptions);

        counters.forEach(counter => observer.observe(counter));
    }

    /**
     * Animate individual counter element
     */
    function animateCounter(element) {
        const target = parseInt(element.textContent);
        const duration = 1500;
        const start = performance.now();
        
        function updateCounter(currentTime) {
            const elapsed = currentTime - start;
            const progress = Math.min(elapsed / duration, 1);
            
            // Easing function for smooth animation
            const easeOutQuart = 1 - Math.pow(1 - progress, 4);
            const current = Math.floor(target * easeOutQuart);
            
            element.textContent = current;
            
            if (progress < 1) {
                requestAnimationFrame(updateCounter);
            } else {
                element.textContent = target; // Ensure final value is exact
            }
        }
        
        requestAnimationFrame(updateCounter);
    }

    /**
     * Subtle parallax effects for floating cards
     */
    function initParallaxEffects() {
        const floatingCards = document.querySelectorAll('.floating-cards .card');
        
        if (!floatingCards.length) return;

        let ticking = false;

        function updateParallax() {
            const scrollY = window.pageYOffset;
            
            floatingCards.forEach((card, index) => {
                const speed = 0.02 + (index * 0.01);
                const yPos = scrollY * speed;
                card.style.transform = `translateY(${yPos}px)`;
            });
            
            ticking = false;
        }

        function onScroll() {
            if (!ticking) {
                requestAnimationFrame(updateParallax);
                ticking = true;
            }
        }

        // Throttled scroll listener
        window.addEventListener('scroll', onScroll, { passive: true });
    }

    /**
     * Enhanced hover effects for feature cards
     */
    function initCardHoverEffects() {
        const cards = document.querySelectorAll('.feature-card');
        
        cards.forEach(card => {
            card.addEventListener('mouseenter', (e) => {
                // Add subtle glow effect
                e.currentTarget.style.setProperty('--glow-opacity', '0.1');
            });
            
            card.addEventListener('mouseleave', (e) => {
                e.currentTarget.style.setProperty('--glow-opacity', '0');
            });
            
            // Mouse move effect for subtle tilt
            card.addEventListener('mousemove', (e) => {
                const rect = card.getBoundingClientRect();
                const x = e.clientX - rect.left;
                const y = e.clientY - rect.top;
                const centerX = rect.width / 2;
                const centerY = rect.height / 2;
                
                const rotateX = (y - centerY) / 10;
                const rotateY = (centerX - x) / 10;
                
                card.style.transform = `perspective(1000px) rotateX(${rotateX}deg) rotateY(${rotateY}deg) translateY(-8px)`;
            });
            
            card.addEventListener('mouseleave', () => {
                card.style.transform = '';
            });
        });
    }

    /**
     * Smooth scrolling for anchor links
     */
    function initSmoothScrolling() {
        const links = document.querySelectorAll('a[href^="#"]');
        
        links.forEach(link => {
            link.addEventListener('click', (e) => {
                const href = link.getAttribute('href');
                if (href === '#') return;
                
                const target = document.querySelector(href);
                if (target) {
                    e.preventDefault();
                    target.scrollIntoView({
                        behavior: 'smooth',
                        block: 'start'
                    });
                }
            });
        });
    }

    /**
     * Dynamic background particles (optional enhancement)
     */
    function initBackgroundParticles() {
        const heroSection = document.querySelector('.hero-section');
        if (!heroSection) return;

        // Create particle container
        const particleContainer = document.createElement('div');
        particleContainer.className = 'particle-container';
        particleContainer.style.cssText = `
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            overflow: hidden;
            pointer-events: none;
            z-index: 1;
        `;
        
        heroSection.appendChild(particleContainer);

        // Create particles
        for (let i = 0; i < 20; i++) {
            createParticle(particleContainer);
        }
    }

    /**
     * Create individual particle element
     */
    function createParticle(container) {
        const particle = document.createElement('div');
        const size = Math.random() * 4 + 2;
        const x = Math.random() * 100;
        const y = Math.random() * 100;
        const duration = Math.random() * 20 + 10;
        
        particle.style.cssText = `
            position: absolute;
            width: ${size}px;
            height: ${size}px;
            background: rgba(102, 126, 234, 0.1);
            border-radius: 50%;
            left: ${x}%;
            top: ${y}%;
            animation: float ${duration}s ease-in-out infinite;
        `;
        
        container.appendChild(particle);
    }

    /**
     * Performance monitoring and optimization
     */
    function initPerformanceOptimizations() {
        // Lazy load non-critical animations
        const lazyAnimations = document.querySelectorAll('[data-lazy-animate]');
        
        if ('IntersectionObserver' in window && lazyAnimations.length) {
            const lazyObserver = new IntersectionObserver((entries) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        const element = entry.target;
                        const animationType = element.dataset.lazyAnimate;
                        
                        element.classList.add(`animate-${animationType}`);
                        lazyObserver.unobserve(element);
                    }
                });
            });
            
            lazyAnimations.forEach(el => lazyObserver.observe(el));
        }

        // Reduce animations on low-performance devices
        if ('deviceMemory' in navigator && navigator.deviceMemory < 4) {
            document.documentElement.classList.add('reduced-animations');
        }
    }

    /**
     * Accessibility enhancements
     */
    function initAccessibilityFeatures() {
        // Skip animations for users who prefer reduced motion
        if (window.matchMedia('(prefers-reduced-motion: reduce)').matches) {
            document.documentElement.classList.add('reduced-motion');
        }

        // Enhanced keyboard navigation for cards
        const interactiveCards = document.querySelectorAll('.feature-card');
        
        interactiveCards.forEach(card => {
            card.setAttribute('tabindex', '0');
            
            card.addEventListener('keydown', (e) => {
                if (e.key === 'Enter' || e.key === ' ') {
                    const link = card.querySelector('a');
                    if (link) {
                        e.preventDefault();
                        link.click();
                    }
                }
            });
        });
    }

    /**
     * Error handling and fallbacks
     */
    window.addEventListener('error', (e) => {
        console.warn('SAED Homepage: Non-critical error occurred:', e.message);
        // Graceful degradation - ensure basic functionality works
    });

    // Export for potential external use
    window.SAEDHomepage = {
        init: init,
        version: '1.0.0'
    };

})();