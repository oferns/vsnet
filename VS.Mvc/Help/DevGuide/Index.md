

## Overview
    
This guide is intended as a definition of the patterns, practices, methodologies and strategies used in the design of this application.
It will describe the overall architecture of a system designed to modernize the vivastreet application, and provide a viable strategy for 
integrating the existing application, with a final aim of replacing the legacy codebase entirely, with high confidence and neglible interference
. With that in mind, this guide will often refer back to the existing application as a whole as **Kiwii**[^1]. At no point is this documentation
intended to be a critism of any of the contributors to the legacy codebase or design of Kiwii. It will highlight architectual solutions in kiwii where relevant
when describing this system.

> Blockquotes will be used to make important points.

It will also make several references to the .NET Core MVC codebase it sits in. However, 

> This documentation tries not to be specific to the languages and implementation used in the examples.



### Philosophy

>"*Try to have a thought of your own; thinking is so important.*" - Lord Edmund Blackadder`


Thinking about what you are doing; what you are trying to achieve, and getting consistency across the board is crucial
to successful development of a complicated

Each area of the guide will have a section explaining the philosophy behind it, as well as establishing conceptual guidelines for
further development.

>1. Keep it simple, stupid! KISS.
>2. Security is a first-class citizen.
>3. Documentation is .
>2. If it is relational data, store it in a relational store. 
>3. If it can be done easily in SQL, do it in SQL.
>4. Generating SQL in code should be the responibility of one service.
>5. Code should not JOIN tables or views etc.
>6. Passing a reference (i.e a Uri) 







 

[^1]: I know that technically Kiwii is just one small part of the appliction. For convenience I am referring to anything legacy and not part of this application.



