document.addEventListener('DOMContentLoaded', function() {
    const codeInput = document.getElementById('code-input');
    const parseButton = document.getElementById('parse-button');
    const astContainer = document.getElementById('ast-container');

    // Parse button click handler
    parseButton.addEventListener('click', function() {
        const code = codeInput.value.trim();
        if (!code) {
            astContainer.innerHTML = '<div class="alert alert-warning">Please enter some code</div>';
            return;
        }

        fetch('/api/ast/parse', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ code: code })
        })
        .then(response => {
            if (!response.ok) {
                throw new Error(`Error! Status: ${response.status}`);
            }
            return response.text();
        })
        .then(data => {
            astContainer.innerHTML = '<pre>' + data + '</pre>';
        })
    });
});
