$(function () {
    /* ---------- SELECT2 ---------- */
    $('.select2-editable').select2({
        theme: 'bootstrap-5',
        width: '100%',
        placeholder: function () { return $(this).data('placeholder'); },
        allowClear: true
    });

    /* ---------- SAVE NEW ITEM ---------- */
    $('.save-btn').on('click', function () {
        const $li = $(this).closest('li');
        const field = $(this).data('field');
        const $input = $li.find('.new-input');
        const value = $input.val().trim();
        const $err = $li.find('.error-msg');

        if (!value) { $err.text('Enter a value'); return; }

        // ----> replace with your real endpoint
        const $select = $('#dropdown-' + field + '');
        const $hiddenInput = $('input[name="' + field + 'Id_New"]');
        const newOption = new Option(value, value, true, true);
        $select.append(newOption).trigger('change'); // For select2 support


    });

    /* ---------- CANCEL (just clear & close) ---------- */
    $('.cancel-btn').on('click', function () {
        const $li = $(this).closest('li');
        $li.find('.new-input').val('');
        $li.find('.error-msg').text('');
        bootstrap.Dropdown.getInstance($li.closest('.dropdown').find('.dropdown-toggle')[0]).hide();
    });






});





// Assume: pageData is your full dataset array
// tableBody is <tbody id="tableBody">

