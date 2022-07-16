// node sample1.js

function convertInchToMeter(inches)  {
    var feet = inches / 12;  //  These five statements are in a block.
    var miles = feet / 5280;
    var nauticalMiles = feet / 6080;
    var cm = inches * 2.54;
    meters = inches / 39.37;
}

var meters = 100;
var km = meters / 1000;  //  These three statements are not in a block.
convertInchToMeter(meters);
console.log( meters );
