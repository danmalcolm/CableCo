
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_Account__Subscriptions__AccountId]') AND parent_object_id = OBJECT_ID('Subscription'))
alter table Subscription  drop constraint FK_Account__Subscriptions__AccountId


    if exists (select * from dbo.sysobjects where id = object_id(N'Account') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Account

    if exists (select * from dbo.sysobjects where id = object_id(N'Product') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Product

    if exists (select * from dbo.sysobjects where id = object_id(N'Subscription') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Subscription

    create table Account (
        AccountId UNIQUEIDENTIFIER not null,
       Version INT not null,
       Code NVARCHAR(20) not null unique,
       primary key (AccountId)
    )

    create table Product (
        ProductId UNIQUEIDENTIFIER not null,
       Version INT not null,
       Code NVARCHAR(20) not null unique,
       primary key (ProductId)
    )

    create table Subscription (
        SubscriptionId UNIQUEIDENTIFIER not null,
       Version INT not null,
       AccountId UNIQUEIDENTIFIER not null,
       ProductCode NVARCHAR(20) not null,
       primary key (SubscriptionId)
    )

    alter table Subscription 
        add constraint FK_Account__Subscriptions__AccountId 
        foreign key (AccountId) 
        references Account
