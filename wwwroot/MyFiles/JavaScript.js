const selectTrigger = document.querySelector('.custom-select-trigger');
const options = document.querySelectorAll('.custom-option');
const hiddenInput = document.getElementById('category-input');
const giftMakerCheckbox = document.getElementById('f-option');

giftMakerCheckbox.addEventListener('change', function () {
    if (this.checked) {
        selectTrigger.parentNode.style.display = 'block';
    } else {
        selectTrigger.parentNode.style.display = 'none';
        // Reset the selected category when hiding the options
        options.forEach(option => {
            option.classList.remove('selected');
        });
        selectTrigger.textContent = 'Select Category';
        hiddenInput.value = '';
    }
});

selectTrigger.addEventListener('click', function () {
    this.parentNode.querySelector('.custom-options').classList.toggle('open');
});

options.forEach(option => {
    option.addEventListener('click', function () {
        if (!this.classList.contains('selected')) {
            this.parentNode.querySelectorAll('.custom-option').forEach(option => {
                option.classList.remove('selected');
            });
            this.classList.add('selected');
            selectTrigger.textContent = this.textContent;
            hiddenInput.value = this.dataset.value;
        }
        this.closest('.custom-options').classList.remove('open');
    });
});

document.addEventListener('click', function (e) {
    if (!selectTrigger.contains(e.target)) {
        document.querySelectorAll('.custom-options').forEach(options => {
            options.classList.remove('open');
        });
    }
});

