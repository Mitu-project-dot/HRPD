function ValidateAge(source, arguments) 
{
    var birthDate = new Date(arguments.Value);

    var currentDate = new Date();

    var currentYear = currentDate.getFullYear();

    var birthYear = birthDate.getFullYear();

    var currentMonth = currentDate.getMonth();

    var birthMonth = birthDate.getMonth();

    var currentDay = currentDate.getDate();

    var birthDay = birthDate.getDate();

    if ((currentYear - birthYear) < 18) 
    {
        arguments.IsValid = false;
    }
    else if (((currentYear - birthYear) == 18) && (currentMonth < birthMonth)) 
    {
        arguments.IsValid = false;
    }
    else if (((currentYear - birthYear) == 18) && (currentMonth == birthMonth) && (currentDay < birthDay)) 
    {
        arguments.IsValid = false;
    }
    else 
    {
        arguments.IsValid = true;
    }

}
