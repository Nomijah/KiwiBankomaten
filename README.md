# KiwiBankomaten
## Description
KiwiBankomaten is an object oriented C# program made to function as a digital bank, as a school group project.
## Visuals 
Visuals of the main menu and the ascii art logo
![Skärmbild_20221215_100722](https://user-images.githubusercontent.com/114058073/207826125-acbc8822-2e5c-47b7-8094-8b58bd270c01.png)
## Usage and functions
User functions include being able to log in and out, see bank accounts/loan accounts and the balances, transfer money between users and internally, see account history, create new accounts and loan money from the bank. The user can see the maximum amount available to loan from the bank. The user can see the interest rate and what payback amount the customer will receive after specified time periods.

Admin functions include being able see the see the complete lists of admins, users, currencies and account types available in the bank. Admins can also create new users, update the currencies, edit user accounts and edit the bank account types avilable.

Further functions in the program include: Users can choose currency when creating accounts. The transfers between accounts with different currencies are converted according to current exchange rates. The currencies are updated by the admins. Login attempts are limited to three tries and blocks the user until admin gives access. The program saves the data from the program database in local textfiles, which can be read as a database. The changes are written in the database between program runs. The users must login using a unique username. The program looks good esthetically with clean menus, coloring and a designed ascii art logo.
## Authors and acknowledgment
The creators are: Charlie (https://github.com/Challeskib), Daniel (https://github.com/DanielDarwiche), Petter (https://github.com/Nomijah), Max (https://github.com/MaxDBerg), Jonathan (https://github.com/JonathanLudvigsson)
## Project status
Finished
## License
MIT License

Copyright (c) [2022] [KiwiBankomaten]
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
