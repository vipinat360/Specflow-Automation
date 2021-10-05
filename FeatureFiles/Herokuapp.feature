Feature: HerokuappAPIDEMO
Background: 
	Given I login to Herokuapp application to get Token using "admin" and "password123"
	When I add following Header in the request
	|Header | Value|
	|Content-Type | application/json|
	|Accept | application/json|

@smoke
Scenario: Scenario_01_GET_Filter List of books by First & Last name
	When I add following parameter in the request for endpoint "/booking"
	|Parameter|Value|
	|firstname | Mark|
	|lastname | Smith|
	And I submit "GET" request to the "" endpoint
	Then I should see "200" as status code
	Then I should see "OK" as status line
@done
Scenario: Scenario_02_GET_Get List of Books
	When I submit "GET" request to the "/booking" endpoint
	Then I should see "200" as status code
	Then I should see "OK" as status line

Scenario: Scenario_03_POST_Create new Book
	When I add following body in request
	| type | Body |
	|Json  | {"firstname" : "Naved","lastname" : "Ali","totalprice" : 111,"depositpaid" : true,"bookingdates" : {"checkin" : "2021-01-01","checkout" : "2021-07-08"},"additionalneeds" : "Breakfast"}  |
	And I submit "POST" request to the "/booking" endpoint
	Then I should see "200" as status code
	Then I should see "OK" as status line
	And I store "bookingid" attribute value in "bookingid" variable for future use

Scenario: Scenario_04_GET_Verify the newly created Book details
	When I submit "GET" request to the "/booking/{bookingid}" endpoint
	Then I should see "200" as status code
	Then I should see "OK" as status line
	Then I verify "bookingid" is "not" available in the response
	Then I verify "firstname" is "" available in the response
	Then I verify attribute "firstname" has "Naved" value in response
	Then I verify attribute "lastname" has "Ali" value in response
	Then I verify attribute "bookingdates.checkin" has "2021-01-01" value in response
	Then I verify attribute "bookingdates.checkout" has "2021-07-08" value in response

Scenario: Scenario_05_PUT_Update newly created book using PUT request
	When I add following Header in the request
	|Header | Value|
	|Cookie | token={token}|
	And I add following body in request
	| type | Body |
	|Json  | {"firstname" : "NA","lastname" : "AL","totalprice" : 123,"depositpaid" : true,"bookingdates" : {"checkin" : "2021-10-10","checkout" : "2021-12-12"},"additionalneeds" : "Breakfast"}  |
	And I submit "PUT" request to the "/booking/{bookingid}" endpoint
	Then I should see "200" as status code
	Then I should see "OK" as status line
	
Scenario: Scenario_06_GET_VerifyTheNewlyUpdatedBookDetails
	When I submit "GET" request to the "/booking/{bookingid}" endpoint
	Then I should see "200" as status code
	Then I should see "OK" as status line
	Then I verify attribute "firstname" has "NA" value in response
	Then I verify attribute "lastname" has "AL" value in response
	Then I verify attribute "totalprice" has "123" value in response
	Then I verify attribute "bookingdates.checkin" has "2021-10-10" value in response
	Then I verify attribute "bookingdates.checkout" has "2021-12-12" value in response


Examples: 

| ResponseCode |
| 200          |