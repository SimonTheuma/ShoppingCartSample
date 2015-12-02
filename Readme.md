Currency

Id
Code
Symbol

-

Currency Lookup Table

SourceCurrency
TargetCurrency
ConversionValue - (Ratio)

-

Product

Id - string/int
Price - decimal (this is base price, will be converted if currency is changed)
Name - string
Description - string
Image - string (will be a link)
Tags - array of strings (used for search)
QuantityAvailable - int

-

Order

ProductId - string/int
UnitPrice - decimal
Quantity - int
SubTotal - unitprice * quantity

-

Charge

Description - string
FlatAmount - decimal
PercentageAmount - decimal

-

DeliveryCharge : Charge

Type: Enum (Standard, Priority)

-

Cart

UserId? - non-nullable
Orders - List of orders
ExtraCharges - List of charges
Discounts - List of discounts
Total - mapped list of order subtotals
LastUpdated - allows you to follow up customers that haven't shopped in a while or had pending items that weren't checked out.
IsCheckedOut - bool
IsCleared - bool (allows you to check if the cart was cleared or not)

-

User

id - int/string
Username - string
Password - string
Email - string
IsTemporary - bool

----------------

Features:

Login system? use identity with sql compact
Allow system to be used when user is not registered, allow him to register on checkout 
	- add temporary users so that anonymous people can still generate user behaviour data
	- offer a perk for a first time buy when user signs up, similar to what vistaprint does - a free item of some sort.
Audits? - Good for checking user behaviour if every action is logged (eg. add to cart, removed from cart)
Add a delivery charge to an order?
Dual / multiple currency pricing
	- Change currency, get new rate from server and apply it to all the pricing
Collaborative filter / recommendation system! :) (if there is enough time, and there probably won't be.)

Adjust cart accordingly if the product becomes out of stock while the user is still shopping?

Use angular.
Use toastr for notifications.
Use unity, DI the repository

-----------------

Site Validation:

Cannot order more stock than is available
Cannot order 0 of something
0 stock is displayed as 'out of stock'

-----------------

Controllers:

CurrencyController (currencies would be adjusted for tax and so on in the lookup, makes it a simple conversion)
	- GetNewPriceModifier(source, target)

ProductController	
	- GetProducts (paging info?)

CartController
	- Get(userId) - if userid is null then return what's in the session, if anything.
	- AddOrder(order)
	- EditOrder(order)
	- Clear(userId)
	- Transfer(sourceUserId, targetUserId, overwrite) - to move the cart once the user has signed up. sourceUser can only be temporary. cart can be overwritten if the user chooses so.

UserController
	- CreateTemporaryUser
	- [Rest of identity stuff]

-----------------

Services:

AuditService - logs the user's actions in the db.
CurrencyService - allows the system to manipulate currencies
ProductService - gets products, checks if product is in stock and also updates stock when products are bought
CartService - manages cart state
UserService - identity stuff

-----------------

Repositories:

AuditRepository
CurrencyRepo?
ProductRepository
CartRepository
UserRepository - identity stuff

-----------------

Config:

BaseCurrency - assumes that all prices are in that currency.

-----------------

User Experience:

Browsing => Pre-Checkout/Confirmation (displayed inventory of items + enter discount codes) => Checkout (success page)