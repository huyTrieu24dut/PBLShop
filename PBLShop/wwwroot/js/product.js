function formatCurrency(number) {
    var reverse = String(number).split('').reverse().join('');
    var result = [];

    for (var i = 0; i < reverse.length; i++) {
        if (i % 3 === 0 && i !== 0) {
            result.push('.');
        }
        result.push(reverse[i]);
    }

    var number = result.reverse().join('');
    return number + ' đ';
}
