let countdown = 5;
const countdownElement = document.getElementById('countdown');
const progressBar = document.getElementById('progressBar');

// Redirect authenticated users back to dashboard

function getRedirectTarget(role) {
    switch (role) {
        case 'admin':
            return '/admindashboard';
        default:
            return '/dashboard'
    }
}

const redirectTarget = getRedirectTarget(typeof userRole !== "undefined" ? userRole : "user");

const updateProgress = () => {
    const percentage = ((5 - countdown) / 5) * 100;
    progressBar.style.width = percentage + '%';
};

const interval = setInterval(() => {
    countdown--;
    countdownElement.textContent = countdown;
    updateProgress();

    if (countdown <= 0) {
        clearInterval(interval);
        window.location.href = redirectTarget;
    }
}, 1000);

updateProgress();
