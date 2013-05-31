using System;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;

namespace CableCo.Accounts.NHibernate
{
    /// <summary>
    /// Adds model mapping to NHibernate Configuration
    /// </summary>
    public static class ModelMapping
    {
        public static void Add(Configuration configuration)
        {
            var mapper = new ModelMapper();

            mapper.BeforeMapClass += MapClass;
            mapper.BeforeMapProperty += MapProperty;
            mapper.BeforeMapManyToOne += MapManyToOne;
            mapper.BeforeMapBag += MapBag;

            mapper.Class<Account>(@class =>
            {
                @class.Property(x => x.Code, prop =>
                {
                    prop.Length(20);
                    prop.Unique(true);
                });
                @class.Bag(x => x.Subscriptions,
                    bag => bag.Cascade(Cascade.All | Cascade.DeleteOrphans),
                    relationShip => relationShip.OneToMany());
            });

            mapper.Class<Product>(@class =>
            {
                @class.Property(x => x.Code, prop =>
                {
                    prop.Length(20);
                    prop.Unique(true);
                });
                @class.Property(x => x.Name, prop =>
                {
                    prop.Length(100);
                });
            });

            mapper.Class<Subscription>(@class =>
            {
                @class.ManyToOne(x => x.Account);
                @class.Property(x => x.ProductCode, prop => prop.Length(20));
                @class.Property(x => x.Status);
            });

            configuration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
        }
        
        private static void MapClass(IModelInspector modelinspector, Type type, IClassAttributesMapper @class)
        {
            if (typeof(Entity).IsAssignableFrom(type))
            {
                @class.Id(type.GetProperty("Id"), id =>
                {
                    id.Column(type.Name + "Id");
                    id.Generator(Generators.GuidComb);
                    id.UnsavedValue(Guid.Empty);
                });
                @class.Version(type.GetProperty("Version"), version => {});
            }
        }

        private static void MapManyToOne(IModelInspector modelinspector, PropertyPath member, IManyToOneMapper manyToOne)
        {
            var columnName = member.LocalMember.GetPropertyOrFieldType().Name + "Id";
            manyToOne.Column(columnName);
            string foreignKey = string.Format("FK_{0}__{1}__{2}", member.LocalMember.ReflectedType.Name, member.LocalMember.Name,
                columnName);
            manyToOne.ForeignKey(foreignKey);
            manyToOne.Cascade(Cascade.Persist);
            manyToOne.NotNullable(true);
        }

        private static void MapProperty(IModelInspector modelinspector, PropertyPath member, IPropertyMapper prop)
        {
            prop.NotNullable(true);
        }

        private static void MapBag(IModelInspector modelinspector, PropertyPath member, IBagPropertiesMapper bag)
        {
            bag.Key(key =>
                {
                    string keyColumnName = string.Format("{0}Id", member.LocalMember.ReflectedType.Name);
                    key.Column(keyColumnName);
                    string foreignKey = string.Format("FK_{0}__{1}__{2}", 
                        member.LocalMember.ReflectedType.Name, member.LocalMember.Name, keyColumnName);
                    key.ForeignKey(foreignKey);
                });
            bag.Fetch(CollectionFetchMode.Subselect);
            bag.Cascade(Cascade.Persist);
            bag.Inverse(true);
        }

    }
}