Hello engineerings!! I divided this file in 3 sections

1. Project description
2. How to run and open the project
3. General considerations

1. Project description

Well, when I started the project, I created a lot of abstractions, inferfaces, extensions, helpers, some new layers, etc. I thought that I should , in some way, show that I knew differente concepts, theories, etc. But that is not the way I think IT projects. It's like a card game: it's not about use all your cards, but use the right cards in the right time. So I gave two steps back, refactory the code.

So, in my project I had some 'layers' based on DDD:

Domain application - the services here don't have any domain logic. For example, the only class here it is used to read the file content and initializate the right class: account ou transaction creator

Domain services - Once I read that domain services are 'things that our domain should do but we can't fit in any entity ou value object'. But, in this case, all classes here offers to my domain application a way to work with my models. They also points to our REPOSITORY.  The repository in the domain services follows the Dependency Inversion of SOLID .


Models - This is my mainly models of the system, where all entities should implement my abstract class ENTITY. Any ENTITY class knows holds your own errors and they know how to auto validate (in this case, ACCOUNT class). The relationship between ENTITY and ACCOUNT follows the liskov substitution principle of SOLID.

Repositories - I know we should not persist the data, but I used a simple memory cache implementation as a REPOSITORY. I hope that is not be a problem. My repository class uses 

CrossCuting - Any support class stays in this folder. Here I created helpers and extensions to avoid DRY (don't repeat yourself) and acelerate my development. 

Also, I used resources to hold the names of the exceptions. I agree that use resource could not be the best way to hold this kind of information, but in this case I think that fits very well. Another idea was to put in the Account class, but I think I could affect the Single Responsibility of SOLID .

When I was developing, I use TDD to create the TRANSACTION CREATOR class. So , I first created all test cases, and imaginate how I would like to use the class. After that, I started the development. 

2. How to run and open the project