Implemented using MVC, Entity Framework, Unity, Identity, AngularJS + Bootstrap

Features Implemented:

- Login system, injected Identity using Unity
- Audit logs for user actions
- Anonymous cart that is automatically transferred upon login/registration.  User can add items to the cart, then login and checkout.
- Validations for stock control upon adding an item to the cart + upon checkout
- Ability to add to cart off the home page
- Typical cart functionality, including adding items to cart, removing items, clearing items, checking out, etc.

Features Considered:

- Currency conversion (kind of set up from the back-end, but not implemented on the front-end)
- Discounts / extra charges to be applied on confirmation/checkout
- Updating order amounts 
- Nicer UI (sorry :( )
- Using session storage

----------------

Architecture:

- Authentication Layer -> References to Identity, including its own services, objects etc.
- Domain Layer -> Class definitions, exceptions, enums, etc.
- Data/Repository Layer -> Where actual calls are made to EF and objects returned
- Service Layer -> Acts as the interface between the data layer + the controller layer
- Controller Layer -> Makes calls to the service layer + returns JSON to the view
- View Layer -> AngularJS application

----------------

Characteristics:

- Server-side persistence for the cart was used as opposed to client-side (eg. session storage) to be able to retain the cart across browsers/pcs 
	& still be able to transfer.
- A lot of custom exceptions were used (eg. ProductOutOfStockException), try-catches located in the controller layer to catch and handle individual 
	exception types.
- User Actions (and their subsequent logs) were implemented by defining a base abstract class that defines an abstract method to get the UserActionType.
	Individual actions inherited from the base, implemented the abstract method & also overrode the ToString() to return a custom message.  A user action log
	item takes the abstract class in the constructor, and generates the log by calling the appropriate methods.