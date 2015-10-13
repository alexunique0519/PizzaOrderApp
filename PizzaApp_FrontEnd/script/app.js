'use strict';

var app = angular.module('PizzaOrderApp', ['ngRoute']);




app.controller('pizzaOrderController', [ '$scope', '$http', function( $scope,$http){

       
    $scope.onSubmit = function(){
         getValue();
        if(validateData())
        {
            var newOrder = new PizzaOrder(customerName, postalCode, city, province, telephoneNumber, email, pizzaSize, toppings, crustType);
            $http.post(serverBase + "api/pizzaOrder/order", newOrder).then(function(response){
                $scope.OrderedItems = response.data.orderedItems;
                $scope.subTotal = response.data.subTotal;
                $scope.totalCost = response.data.totalCost;
            
                //
                document.getElementById("resultArea").classList.remove("hidden");
                document.getElementById("message").innerHTML= response.data.message;
                window.scrollTo(0,document.body.scrollHeight);

            });   
        }
    
    }

}]);


var serverBase = "http://localhost:54301/";

//define all the values 
var customerName;
var postalCode;
var city;
var province;
var telephoneNumber;
var email;
var pizzaSize;
var toppings;
var crustType;


function PizzaOrder (customerName, postalCode, city, province, telephoneNumber, email, pizzaSize, toppings, crustType){
    
    this.customerName = customerName;
    this.postalCode = postalCode;
    this.city = city;
    this.province = province;
    this.telephoneNumber = telephoneNumber;
    this.email = email;
    this.pizzaSize = pizzaSize;
    this.toppings = toppings;
    this.crustType = crustType;
    
}

//get all the data from the page
function getValue(){
	
    customerName = document.getElementById("customerName").value;
    postalCode = document.getElementById("postalCode").value;
    city = document.getElementById("city").value;
    province = document.getElementById("province").value;
    telephoneNumber = document.getElementById("telephoneNumber").value;
    email = document.getElementById("email").value;
    pizzaSize = document.getElementById("pizzaSize").value;
    crustType = document.getElementById("crustType").value;
    
    toppings = getCheckedBoxes("pizzaToppings");
}


//this is for getting all the checked checkboxes 
function getCheckedBoxes(chkboxName) {
  var checkboxes = document.getElementsByName(chkboxName);
  var checkboxesChecked = [];
  // loop over them all
  for (var i=0; i<checkboxes.length; i++) {
     // And stick the checked ones onto an array...
     if (checkboxes[i].checked) {
        checkboxesChecked.push(checkboxes[i].value);
     }
  }
  // Return the array if it is non-empty, or null
  return checkboxesChecked.length > 0 ? checkboxesChecked : null;
}

/*
window.onload =function(){
    
  document.getElementById("submit").onclick = function(){
        getValue();
        if(validateData())
        {
            var newOrder = new PizzaOrder(customerName, postalCode, city, province, telephoneNumber, email, pizzaSize, toppings, crustType);
            
        }
    }
}
*/

function validateData()
{
    //validate the customerName field
    if(isEmpty(customerName))
    {
        showErrorMessage("nameError", "please enter your name");
        return false;
    }
    else
    {
         document.getElementById("nameError").classList.add("hidden");
    }
    //validata postal code field
    if(isEmpty(postalCode))
    {
        showErrorMessage("postalCodeError", "Please enter your postal code.");
        return false;
    }
    if(!isPostalCodeValid(postalCode))
    {
         showErrorMessage("postalCodeError", "Please enter a valid canadian postal code(eg. N2C 2H7).");
         return false;
    }
    else
    {
        document.getElementById("postalCodeError").classList.add("hidden");
    }
    //validate the province field
    if(isEmpty(province))
    {
         showErrorMessage("provinceError", "Please select a province");
         return false;
    }
    else
    {
        document.getElementById("provinceError").classList.add("hidden");
    }
    //validate city field
    if(isEmpty(city))
    {
        showErrorMessage("cityError", "Please enter the name of your city");
        return false;
    }
    else
    {
        removeErrorMessage("cityError");
    }
    //validate telephone number field
    if(isEmpty(telephoneNumber))
    {
        showErrorMessage("telephoneNumberError", "Please enter your telephone number");
        return false;
    }
    if(!isPhoneNumberValid(telephoneNumber))
    {
        showErrorMessage("telephoneNumberError", "Please enter a 10-digit valid telephone number.");
        return false;
    }
    else
    {
        removeErrorMessage("telephoneNumberError");
    }
    //validate email 
    if(isEmpty(email))
    {
        showErrorMessage("emailError", "Please enter your email address");
        return false;
    }
    if(!isEmailAddressValid(email))
    {
        showErrorMessage("emailError", "Please enter a valid email address");
        return false;
    }
    else
    {
        removeErrorMessage("emailError");
    }
    //validate the pizzaSize field
    if(isEmpty(pizzaSize))
    {
        showErrorMessage("pizzaSizeError", "Please select the pizza size.");
        return false;
    }
    else
    {
        removeErrorMessage("pizzaSizeError");
    }
    //validate the pizzaToppings filed    
    if(toppings == null)
    {
        showErrorMessage("toppingsError", "Please select at least one topping you like.");
        return false;
    }
    else
    {
        removeErrorMessage("toppingsError");
    }
    if(isEmpty(crustType))
    {
        showErrorMessage("crustTypeError", "Please select a crust type you like.")
        return false;
    }
    else
    {
        removeErrorMessage("crustTypeError")
    }
    
    return true;
}


function showErrorMessage(elementId, message){
     document.getElementById(elementId).classList.remove("hidden");
     document.getElementById(elementId).innerHTML= message;
     document.getElementById(elementId).classList.add("alert-danger");
}


function removeErrorMessage(elementId)
{
    document.getElementById(elementId).classList.add("hidden");
}



app.config(function ($routeProvider) {

    $routeProvider.when("/index", {
        controller: "pizzaOrderController",
        templateUrl: "/index"
    });
    
 $routeProvider.otherwise({ redirectTo: "/index" });

});