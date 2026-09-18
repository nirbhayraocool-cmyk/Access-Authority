document.addEventListener('DOMContentLoaded', function () {

    document.querySelectorAll('.aa-mega > a, .aa-company > a, .aa-product > a')
        .forEach(function (link) {

            link.addEventListener('click', function (e) {

                if (window.innerWidth < 992) {

                    e.preventDefault();

                    const parent = this.parentElement;

                    // dusre dropdown band
                    document.querySelectorAll('.aa-mega, .aa-company, .aa-product')
                        .forEach(function (item) {
                            if (item !== parent) {
                                item.classList.remove('show');
                            }
                        });

                    parent.classList.toggle('show');
                }

            });

        });

    // Bahar click karne pe sab dropdown band
    document.addEventListener('click', function (e) {
        if (!e.target.closest('.aa-mega, .aa-company, .aa-product')) {
            document.querySelectorAll('.aa-mega, .aa-company, .aa-product')
                .forEach(function (item) {
                    item.classList.remove('show');
                });
        }
    });

});