Error handling was not implemented correctly and could cause null exceptions
Returning a single int instead of a response object is inflexible and does not cater for error that may occur. I would sugest creating a proper object to return with results.
InvalidOperationException is not a nice Exception to handle from the application that sends a request
Casting the application each time you want to determine the type is tedious and innefficiant.
Moving the application types into the Seller Application allows for more flexibility.
