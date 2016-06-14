using System.Collections.Generic;
using MetaBuilder.BusinessLogic;
using f = MetaBuilder.BusinessFacade.Storage.RepositoryTemp;
using b = MetaBuilder.BusinessLogic;

namespace MetaBuilder.BusinessFacade.Storage.RepositoryTemp
{
    public class ItemAndRule
    {
        public ItemAndRule(RepositoryRule Rule, IRepositoryItem Item)
        {
            this.rule = Rule;
            this.item = Item;
            EmbeddedItemRules = new List<ItemAndRule>();
        }

        private RepositoryRule rule;
        public RepositoryRule Rule
        {
            get { return rule; }
            set { rule = value; }
        }

        private IRepositoryItem item;
        public IRepositoryItem Item
        {
            get { return item; }
            set { item = value; }
        }
	
        private List<ItemAndRule> embeddedItemRules;

        public List<ItemAndRule> EmbeddedItemRules
        {
            get { return embeddedItemRules; }
            set { embeddedItemRules = value; }
        }
	
    }
}
