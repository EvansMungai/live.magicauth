let countdown = 5;
const countdownElement = document.getElementById('countdown');
const progressBar = document.getElementById('progressBar');

// Get returnUrl from query string
const urlParams = new URLSearchParams(window.location.search);
const returnUrl = urlParams.get('returnUrl') || '/Dashboard';

// Update progress bar width
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
        redirectToLogin();
    }
}, 1000);

function redirectToLogin() {
    window.location.href = '/auth/login?returnUrl=' + encodeURIComponent(returnUrl);
}

// Initialize progress
updateProgress();