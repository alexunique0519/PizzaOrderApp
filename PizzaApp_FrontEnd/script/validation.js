function isEmpty(val){
    
    if(val.trim() == "")
        return true;
    
    return false;
}

function isPostalCodeValid(postalCode){
    
    var regex = new RegExp(/^[ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ]( )?\d[ABCEGHJKLMNPRSTVWXYZ]\d$/i);
    if (regex.test(postalCode))
        return true;
    else 
        return false;
}

function isPhoneNumberValid(telephoneNumber){
    var regex = new RegExp(/^\(?([1-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/);
    if(regex.test(telephoneNumber))
       return true;
    else 
       return false;
}

function isEmailAddressValid(email){
    var regex = new RegExp(/^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i);
    if (regex.test(email)) 
        return true;
    else
        return false;
}