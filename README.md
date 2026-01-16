#CHANGELOG

## Feature tickets

The basket needs to persist between page loads

	Introduced Protected Local Storage implementation in order to facilitate basket persistence across tabs.

The basket menu needs to be responsive to fit on a mobile device

	Changed the basket CSS to automatically snap to the top of the page on smaller devices

Menu items need to stand out when hovering over

	Simple on hover implementation with a basic animation

Menu items need a category, and this needs to be reflected in the UI (the categories themselves are arbitary. ie: Starter, Main, Dessert)

	Introduced items categories to be displayed in the UI and for the menu items to be sorted accordingly

The restuarant always add £2 as a delivery fee, this needs to be included in the basket total

	Added a basic fee display informing the user of the addition of a £2 fee as well as adding it to the basket total

The basket needs to group the items together using a quanity display
	
	Basket items now stack and show the total amount of items in said stack alongside its total value

Code coverage currently sits at 0%

	Introduced component tests to veryify the integrity of the rendered pages as well as the intended behaviour

## Bug tickets

When I add something to my basket on one tab, it doesn't seem to add them on the other after I refresh?

	Fixed via the Protected Local Storage implementation

The menu items aren't displayed in price order

	All the menu items now are sorted in accordance to their category and price, from lowest to highest

Chocolate Cake's description isn't showing

	This entry was missing its description property, same with one other seeded menu entry

The basket doesn't show £

	Added the pound symbols to prices in the basket

The basket total doesn't add up correctly

	The basket now calculates the total prices correctly, with the addition of the delivery fee
