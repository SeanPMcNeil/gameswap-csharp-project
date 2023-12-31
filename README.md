# GameSwap
### A Video Game Trading Marketplace
### Built using C# .NET 6 MVC and Bootstrap

#### Game Listing Browser
![Browse Listings](https://user-images.githubusercontent.com/23509634/177610832-380e289e-2dd4-4dc6-b885-36a846bde5b4.png)

#### About
The goal of GameSwap is to allow registered users to list games they want to sell and purchase games from other users. Users must register to utilize site features, and provide an address for shipping information. Listings can be created, read, updated, and deleted. There is a messaging system built into the app to allow users to express interest in listed games and to potentially begin negotiations on the listed price.

#### Tech Used
- C# .NET 6
- MySQL
- Bootstrap CSS
- GiantBomb API

#### Highlights
- Messaging system is built from scratch to allow communication between site users. Included negotiation functionality allows for dynamic price adjustments based on both users accepting the same price.
- Game images and descriptions are dynamically called from the GiantBomb API depending on what the page needs to load.

#### Future Considerations
- Refreshed API usage so it does not need to be called so frequently
- Fleshed out "Coins" system to monetize the site. Potential integration with Stripe to allow for user payments to the site to frontload Coins for trades.
- Integrate the USPS API for user address validation/shipping information validation.
- Allow users to upload their own images of listed games.

#### User Dashboard Screenshot
![User Dashboard](https://user-images.githubusercontent.com/23509634/177612427-7bbbfb62-7e41-46c5-ace1-e127ad9362a2.png)
