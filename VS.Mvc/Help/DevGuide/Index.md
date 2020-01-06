

## Overview
    
This guide is intended as a definition of the patterns, practices, methodologies and strategies used in the design of this application.
It will describe the overall architecture of a system designed to modernize the vivastreet application, and provide a viable strategy for 
integrating the existing application, with a final aim of replacing the legacy codebase entirely, with high confidence and neglible interference
. With that in mind, this guide will often refer back to the existing application as a whole as **Kiwii**[^1]. At no point is this documentation
intended to be a critism of any of the contributors to the legacy codebase or design of Kiwii. It will highlight architectual solutions in kiwii where relevant
when describing comparative solutions in this guide.

> Blockquotes will be used to make important points.

It will also make several references to the .NET Core MVC codebase it sits in. However, 

> This documentation tries not to be specific to the languages and implementation used in the examples.

### Philosophy

>"*Try to have a thought of your own; thinking is so important.*" - Lord Edmund Blackadder`

Sharing and defining development philosophy greatly eases the development friction for both new starters and colleagues alike. 
Each area of the guide will have a section explaining the philosophy behind it, as well as establishing conceptual guidelines for
further development. Below are the commandments for the project overall.

>2. Security is a first-class citizen.
>3. Documentation is .

>1. Keep it simple, stupid! KISS.
>2. Dont repeat yourself.
>2. Security is a fist-class citizen.
>3. Testing is a first-class citizen.
>5. Performance is a first-class citizen.
>4. Documentation is a first-class citizen.
>5. Less code is better, but not at the expense of readability.
>6. If it is relational data, store it in a relational store.


The following terms will be used to describe the architecture in this guide.

**Reliable Messaging**
 
This is a term used to describe services such as ApacheMQ and ServiceBus that provide reliable queues and topics. 

>A **queue** means a message goes to one and only one possible subscriber. A **topic** goes to each and every subscriber. 
>**Topics** are for the publisher-subscriber model, while **queues** are for point-to-point 

The architecture in this guide use both topics and queues, and uses ApacheMQ in the implementation.

**Eventual Consistency**

The following programming concepts will be used on server functionlity (ie websites, api).

>Dependency Injection.
>Inversion of Control.
>The Decorator pattern.
>The Mediator pattern.
>Aspect Oriented Programming (AOP).
>Command Query Seperation.


**Dependency Injection**

This is a programming pattern where dependencies are injected into a object/class, as opposed to being created by the class itself.
A related term you will see used in this guide is the **Composition root**.

**Inversion of control**

This is the means to create the dependencies and the

In terms of data storage, we will make clear seperations between relational data and non-relational data, and choose a relevant storage 
platform.

In this solution we have used Postgres as a relational store





 

[^1]: I know that technically Kiwii is just one small part of the appliction. For convenience I am referring to anything legacy and not part of this application.



