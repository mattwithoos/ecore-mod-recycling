using System;
using System.Collections.Generic;
using System.Linq;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Skills;
using Eco.Shared.Localization;
using Eco.Shared.Utils;

namespace Eco.Mods.TechTree
{
    public static class DefaultCosts
    {
        public static float defaultExperienceOnCraft()
        {
            return 1f;
        }
        public static float DefaultCraftMinutes()
        {
            return 3f;
        }
        public static float DefaultLabor()
        {
            return 100f;
        }
        public static Type DefaultSkill()
        {
            return typeof (SurvivalistSkill);
        }
    }

    public class IngredientListStorage
    {
        public static readonly Dictionary<string, IEnumerable<IngredientElement>> OrigIngredientDictionary = new();

    }
    
    public class DefaultRecipeBuilder : RecipeFamily
    {
       
        private readonly List<CraftingElement> _newOutputs = new();
        public List<IngredientElement> NewIngredients = new();

        public static CraftingElement GetOriginalItemFromRecipe(RecipeFamily origRecipe)
        {
            return origRecipe.DefaultRecipe.Items.First();
        }

        public List<CraftingElement> GetValidatedDividedIngredients(IEnumerable<IngredientElement> origIngredientElements, float origRecipeDivide = 3)
        {
            foreach (var ingredient in origIngredientElements)
            {
                var quantity = ingredient.Quantity.GetBaseValue / origRecipeDivide;
                if (ingredient.Tag?.Name == "Wood" && quantity > 0.9 && _newOutputs.Count(x => x.GetType() == typeof(CedarLogItem)) == 0)
                {
                    _newOutputs.Add(new CraftingElement<CedarLogItem>(quantity));
                }
                
                if (ingredient.Tag?.Name == "HewnLog" && quantity > 0.9 && _newOutputs.Count(x => x.GetType() == typeof(SoftwoodHewnLogItem)) == 0)
                {
                    _newOutputs.Add(new CraftingElement<SoftwoodHewnLogItem>(quantity));
                }
            
                if (ingredient.Tag?.Name == "Lumber" && quantity > 0.9 && _newOutputs.Count(x => x.GetType() == typeof(LumberItem)) == 0)
                {
                    _newOutputs.Add(new CraftingElement<LumberItem>(quantity));
                }
                
                if (ingredient.Tag?.Name == "Fabric" && quantity > 0.9 && _newOutputs.Count(x => x.GetType() == typeof(CottonFabricItem)) == 0)
                {
                    _newOutputs.Add(new CraftingElement<CottonFabricItem>(quantity));
                }
                
                if (ingredient.Tag?.Name == "WoodBoard" && quantity > 0.9 && _newOutputs.Count(x => x.GetType() == typeof(BoardItem)) == 0)
                {
                    _newOutputs.Add(new CraftingElement<BoardItem>(quantity));
                }
                
                if (ingredient.Tag?.Name == "Rock" && quantity > 0.9 && _newOutputs.Count(x => x.GetType() == typeof(GraniteItem)) == 0)
                {
                    _newOutputs.Add(new CraftingElement<GraniteItem>(quantity));
                }
                
                if (ingredient.Tag?.Name == "MortaredStone" && quantity > 0.9 && _newOutputs.Count(x => x.GetType() == typeof(MortaredStoneItem)) == 0)
                {
                    _newOutputs.Add(new CraftingElement<MortaredStoneItem>(quantity));
                }
                
                if (ingredient.Item?.GetType() == typeof(NailItem) && quantity > 0.9 && _newOutputs.Count(x => x.GetType() == typeof(NailItem)) == 0)
                {
                    _newOutputs.Add(new CraftingElement<NailItem>(quantity));
                }

                if (ingredient.Item?.GetType() == typeof(PlantFibersItem) && quantity > 0.9 && _newOutputs.Count(x => x.GetType() == typeof(PlantFibersItem)) == 0)
                {
                    _newOutputs.Add(new CraftingElement<PlantFibersItem>(quantity));
                }
            }
            
            _newOutputs.Add(new CraftingElement<GarbageItem>(0.25f));

            return _newOutputs;
        }

        private static string CleanNewRecipeName(string newRecipeName)
        {
            return newRecipeName.RemoveSpaces().Strip('(').Strip(')');
        }

        protected DefaultRecipeBuilder(string newRecipeName, string origRecipeName, Type origItem)
        {
            var origIngredientsList = IngredientListStorage.OrigIngredientDictionary
                .FirstOrDefault(x => x.Key == origRecipeName)
                .Value;
            var recipe = new Recipe();
            recipe.Init(CleanNewRecipeName(newRecipeName), Localizer.DoStr(newRecipeName),
                new List<IngredientElement> {new(origItem, 1, true)},
                GetValidatedDividedIngredients(origIngredientsList));
            Recipes = new List<Recipe> {recipe};
            
            ExperienceOnCraft = DefaultCosts.defaultExperienceOnCraft();
            LaborInCalories = CreateLaborInCaloriesValue(DefaultCosts.DefaultLabor(), DefaultCosts.DefaultSkill());
            CraftMinutes = CreateCraftTimeValue(DefaultCosts.DefaultCraftMinutes());
            Initialize(Localizer.DoStr(newRecipeName), typeof(DisassemblyCampfireBetterRecipe));
            CraftingComponent.AddRecipe(typeof(ToolBenchObject), this);
        }
        
    }

    /**
     * Wood stuff
     */
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyWoodenBowRecipe : DefaultRecipeBuilder
    {
        public DisassemblyWoodenBowRecipe() : base("Disassemble Wooden Bow", nameof(WoodenBowRecipe), typeof(WoodenBowItem))
        {
        }
    }

    public partial class WoodenBowRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(WoodenBowRecipe), this.Ingredients);
        }
    }

    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyCampfireBetterRecipe : DefaultRecipeBuilder
    {
        public DisassemblyCampfireBetterRecipe() : base("Disassembly Campfire (Better) Recipe", nameof(CampfireRecipe), typeof(CampfireItem))
        {
        }
    }

    public partial class CampfireRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(CampfireRecipe), this.Ingredients);
        }
    }
    
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyWoodenHoeItemRecipe : DefaultRecipeBuilder
    {
        public DisassemblyWoodenHoeItemRecipe() : base("Dismantle Wooden Hoe", nameof(WoodenHoeRecipe), typeof(WoodenHoeItem))
        {
        }
    }

    public partial class WoodenHoeRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(WoodenHoeRecipe), this.Ingredients);
        }
    }
    
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyWoodenShovelRecipe : DefaultRecipeBuilder
    {
        public DisassemblyWoodenShovelRecipe() : base("Disassemble Wooden Shovel", nameof(WoodenShovelRecipe), typeof(WoodenShovelItem))
        {
        }
    }

    public partial class WoodenShovelRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(WoodenShovelRecipe), this.Ingredients);
        }
    }
    
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyWoodenWheelRecipe : DefaultRecipeBuilder
    {
        public DisassemblyWoodenWheelRecipe() : base("Disassemble Wooden Wheel", nameof(WoodenWheelRecipe), typeof(WoodenWheelItem))
        {
        }
    }

    public partial class WoodenWheelRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(WoodenWheelRecipe), this.Ingredients);
        }
    }
    
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyWoodenFabricBedRecipe : DefaultRecipeBuilder
    {
        public DisassemblyWoodenFabricBedRecipe() : base("Disassemble Wooden Fabric Bed Recipe", nameof(WoodenFabricBedRecipe), typeof(WoodenFabricBedItem))
        {
        }
    }

    public partial class WoodenFabricBedRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(WoodenFabricBedRecipe), this.Ingredients);
        }
    }
    
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyWoodenStrawBedRecipe : DefaultRecipeBuilder
    {
        public DisassemblyWoodenStrawBedRecipe() : base("Disassemble Wooden Straw Bed Recipe", nameof(WoodenStrawBedRecipe), typeof(WoodenStrawBedItem))
        {
        }
    }

    public partial class WoodenStrawBedRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(WoodenStrawBedRecipe), this.Ingredients);
        }
    }
    
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyLargeHangingWoodSignRecipe : DefaultRecipeBuilder
    {
        public DisassemblyLargeHangingWoodSignRecipe() : base("Disassemble Large Hanging Wood Sign Recipe", nameof(LargeHangingWoodSignRecipe), typeof(LargeHangingWoodSignItem))
        {
        }
    }

    public partial class LargeHangingWoodSignRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(LargeHangingWoodSignRecipe), this.Ingredients);
        }
    }
    
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyLargeStandingWoodSignRecipe : DefaultRecipeBuilder
    {
        public DisassemblyLargeStandingWoodSignRecipe() : base("Disassemble Large Standing Wood Sign Recipe", nameof(LargeStandingWoodSignRecipe), typeof(LargeStandingWoodSignItem))
        {
        }
    }

    public partial class LargeStandingWoodSignRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(LargeStandingWoodSignRecipe), this.Ingredients);
        }
    }
    
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblySmallHangingWoodSignRecipe : DefaultRecipeBuilder
    {
        public DisassemblySmallHangingWoodSignRecipe() : base("Disassemble Small Hanging Wood Sign Recipe", nameof(SmallHangingWoodSignRecipe), typeof(SmallHangingWoodSignItem))
        {
        }
    }

    public partial class SmallHangingWoodSignRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(SmallHangingWoodSignRecipe), this.Ingredients);
        }
    }
    
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblySmallStandingWoodSignRecipe : DefaultRecipeBuilder
    {
        public DisassemblySmallStandingWoodSignRecipe() : base("Disassemble Small Standing Wood Sign Recipe", nameof(SmallStandingWoodSignRecipe), typeof(SmallStandingWoodSignItem))
        {
        }
    }

    public partial class SmallStandingWoodSignRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(SmallStandingWoodSignRecipe), this.Ingredients);
        }
    }
    
    
    /**
     * Lumber-ish stuff
     */
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyLargeHangingLumberSignRecipe : DefaultRecipeBuilder
    {
        public DisassemblyLargeHangingLumberSignRecipe() : base("Disassemble Large Hanging Lumber Sign Recipe", nameof(LargeHangingLumberSignRecipe), typeof(LargeHangingLumberSignItem))
        {
        }
    }

    public partial class LargeHangingLumberSignRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(LargeHangingLumberSignRecipe), this.Ingredients);
        }
    }
    
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyLargeStandingLumberSignRecipe : DefaultRecipeBuilder
    {
        public DisassemblyLargeStandingLumberSignRecipe() : base("Disassemble Large Standing Lumber Sign Recipe", nameof(LargeStandingLumberSignRecipe), typeof(LargeStandingLumberSignItem))
        {
        }
    }

    public partial class LargeStandingLumberSignRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(LargeStandingLumberSignRecipe), this.Ingredients);
        }
    }
    
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblySmallHangingLumberSignRecipe : DefaultRecipeBuilder
    {
        public DisassemblySmallHangingLumberSignRecipe() : base("Disassemble Small Hanging Lumber Sign Recipe", nameof(SmallHangingLumberSignRecipe), typeof(SmallHangingLumberSignItem))
        {
        }
    }

    public partial class SmallHangingLumberSignRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(SmallHangingLumberSignRecipe), this.Ingredients);
        }
    }
    
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblySmallStandingLumberSignRecipe : DefaultRecipeBuilder
    {
        public DisassemblySmallStandingLumberSignRecipe() : base("Disassemble Small Standing Lumber Sign Recipe", nameof(SmallStandingLumberSignRecipe), typeof(SmallStandingLumberSignItem))
        {
        }
    }

    public partial class SmallStandingLumberSignRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(SmallStandingLumberSignRecipe), this.Ingredients);
        }
    }
    
    /**
     * Stone-ish stuff
     */
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyStoneAxeRecipe : DefaultRecipeBuilder
    {
        public DisassemblyStoneAxeRecipe() : base("Disassemble Stone Axe Recipe", nameof(StoneAxeRecipe), typeof(StoneAxeItem))
        {
        }
    }

    public partial class StoneAxeRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(StoneAxeRecipe), this.Ingredients);
        }
    }
    
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyStoneBrazierRecipe : DefaultRecipeBuilder
    {
        public DisassemblyStoneBrazierRecipe() : base("Disassemble Stone Brazier Recipe", nameof(StoneBrazierRecipe), typeof(StoneBrazierItem))
        {
        }
    }

    public partial class StoneBrazierRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(StoneBrazierRecipe), this.Ingredients);
        }
    }
    
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyStoneHammerRecipe : DefaultRecipeBuilder
    {
        public DisassemblyStoneHammerRecipe() : base("Disassemble Stone Hammer Recipe", nameof(StoneHammerRecipe), typeof(StoneHammerItem))
        {
        }
    }

    public partial class StoneHammerRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(StoneHammerRecipe), this.Ingredients);
        }
    }
        
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyStoneMacheteRecipe : DefaultRecipeBuilder
    {
        public DisassemblyStoneMacheteRecipe() : base("Disassemble Stone Machete Recipe", nameof(StoneMacheteRecipe), typeof(StoneMacheteItem))
        {
        }
    }

    public partial class StoneMacheteRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(StoneMacheteRecipe), this.Ingredients);
        }
    }
        
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyStonePickaxeRecipe : DefaultRecipeBuilder
    {
        public DisassemblyStonePickaxeRecipe() : base("Disassemble Stone Pickaxe Recipe", nameof(StonePickaxeRecipe), typeof(StonePickaxeItem))
        {
        }
    }

    public partial class StonePickaxeRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(StonePickaxeRecipe), this.Ingredients);
        }
    }
        
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyStoneSickleRecipe : DefaultRecipeBuilder
    {
        public DisassemblyStoneSickleRecipe() : base("Disassemble Stone Sickle Recipe", nameof(StoneSickleRecipe), typeof(StoneSickleItem))
        {
        }
    }

    public partial class StoneSickleRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(StoneSickleRecipe), this.Ingredients);
        }
    }
        
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyStoneRoadToolRecipe : DefaultRecipeBuilder
    {
        public DisassemblyStoneRoadToolRecipe() : base("Disassemble Stone Road Tool Recipe", nameof(StoneRoadToolRecipe), typeof(StoneRoadToolItem))
        {
        }
    }

    public partial class StoneRoadToolRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(StoneRoadToolRecipe), this.Ingredients);
        }
    }
    
    /**
     * Mortared Stone
     */
        
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyMortaredStoneRecipe : DefaultRecipeBuilder
    {
        public DisassemblyMortaredStoneRecipe() : base("Disassemble Mortared Stone Recipe", nameof(MortaredStoneRecipe), typeof(MortaredStoneItem))
        {
        }
    }

    public partial class MortaredStoneRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(MortaredStoneRecipe), this.Ingredients);
        }
    }
        
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyMortaredStoneBenchRecipe : DefaultRecipeBuilder
    {
        public DisassemblyMortaredStoneBenchRecipe() : base("Disassemble Mortared Stone Bench Recipe", nameof(MortaredStoneBenchRecipe), typeof(MortaredStoneBenchItem))
        {
        }
    }

    public partial class MortaredStoneBenchRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(MortaredStoneBenchRecipe), this.Ingredients);
        }
    }
        
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyMortaredStoneChairRecipe : DefaultRecipeBuilder
    {
        public DisassemblyMortaredStoneChairRecipe() : base("Disassemble Mortared Stone Chair Recipe", nameof(MortaredStoneChairRecipe), typeof(MortaredStoneChairItem))
        {
        }
    }

    public partial class MortaredStoneChairRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(MortaredStoneChairRecipe), this.Ingredients);
        }
    }
        
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyMortaredStoneDoorRecipe : DefaultRecipeBuilder
    {
        public DisassemblyMortaredStoneDoorRecipe() : base("Disassemble Mortared Stone Door Recipe", nameof(MortaredStoneDoorRecipe), typeof(MortaredStoneDoorItem))
        {
        }
    }

    public partial class MortaredStoneDoorRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(MortaredStoneDoorRecipe), this.Ingredients);
        }
    }
        
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyMortaredStoneFireplaceRecipe : DefaultRecipeBuilder
    {
        public DisassemblyMortaredStoneFireplaceRecipe() : base("Disassemble Mortared Stone Fireplace Recipe", nameof(MortaredStoneFireplaceRecipe), typeof(MortaredStoneFireplaceItem))
        {
        }
    }

    public partial class MortaredStoneFireplaceRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(MortaredStoneFireplaceRecipe), this.Ingredients);
        }
    }
        
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyMortaredStoneTableRecipe : DefaultRecipeBuilder
    {
        public DisassemblyMortaredStoneTableRecipe() : base("Disassemble Mortared Stone Table Recipe", nameof(MortaredStoneTableRecipe), typeof(MortaredStoneTableItem))
        {
        }
    }

    public partial class MortaredStoneTableRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(MortaredStoneTableRecipe), this.Ingredients);
        }
    }
        
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyLargeHangingMortaredStoneSignRecipe : DefaultRecipeBuilder
    {
        public DisassemblyLargeHangingMortaredStoneSignRecipe() : base("Disassemble Large Hanging Mortared Stone Sign Recipe", nameof(LargeHangingMortaredStoneSignRecipe), typeof(LargeHangingMortaredStoneSignItem))
        {
        }
    }

    public partial class LargeHangingMortaredStoneSignRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(LargeHangingMortaredStoneSignRecipe), this.Ingredients);
        }
    }
        
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyLargeStandingMortaredStoneSignRecipe : DefaultRecipeBuilder
    {
        public DisassemblyLargeStandingMortaredStoneSignRecipe() : base("Disassemble Large Standing Mortared Stone Sign Recipe", nameof(LargeStandingMortaredStoneSignRecipe), typeof(LargeStandingMortaredStoneSignItem))
        {
        }
    }

    public partial class LargeStandingMortaredStoneSignRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(LargeStandingMortaredStoneSignRecipe), this.Ingredients);
        }
    }
        
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblySmallHangingMortaredStoneSignRecipe : DefaultRecipeBuilder
    {
        public DisassemblySmallHangingMortaredStoneSignRecipe() : base("Disassemble Small Hanging Mortared Stone Sign Recipe", nameof(SmallHangingMortaredStoneSignRecipe), typeof(SmallHangingMortaredStoneSignItem))
        {
        }
    }

    public partial class SmallHangingMortaredStoneSignRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(SmallHangingMortaredStoneSignRecipe), this.Ingredients);
        }
    }
        
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblySmallStandingMortaredStoneSignRecipe : DefaultRecipeBuilder
    {
        public DisassemblySmallStandingMortaredStoneSignRecipe() : base("Disassemble Small Standing Mortared Stone Sign Recipe", nameof(SmallStandingMortaredStoneSignRecipe), typeof(SmallStandingMortaredStoneSignItem))
        {
        }
    }

    public partial class SmallStandingMortaredStoneSignRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(SmallStandingMortaredStoneSignRecipe), this.Ingredients);
        }
    }
    
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyMillRecipe : DefaultRecipeBuilder
    {
        public DisassemblyMillRecipe() : base("Disassemble Mill Recipe", nameof(MillRecipe), typeof(MillItem))
        {
        }
    } 

    public partial class MillRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(MillRecipe), this.Ingredients);
        }
    }
   
        
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyCarpentryTableRecipe : DefaultRecipeBuilder
    {
        public DisassemblyCarpentryTableRecipe() : base("Disassemble Carpentry Table Recipe", nameof(CarpentryTableRecipe), typeof(CarpentryTableItem))
        {
        }
    } 

    public partial class CarpentryTableRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(CarpentryTableRecipe), this.Ingredients);
        }
    }
    
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyToolBenchRecipe : DefaultRecipeBuilder
    {
        public DisassemblyToolBenchRecipe() : base("Disassemble Tool Bench Recipe", nameof(ToolBenchRecipe), typeof(ToolBenchItem))
        {
        }
    } 

    public partial class ToolBenchRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(ToolBenchRecipe), this.Ingredients);
        }
    }
    
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyWorkbenchRecipe : DefaultRecipeBuilder
    {
        public DisassemblyWorkbenchRecipe() : base("Disassemble WorkbenchRecipe", nameof(WorkbenchRecipe), typeof(WorkbenchItem))
        {
        }
    } 

    public partial class WorkbenchRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(WorkbenchRecipe), this.Ingredients);
        }
    }
    
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyMasonryTableRecipe : DefaultRecipeBuilder
    {
        public DisassemblyMasonryTableRecipe() : base("Disassemble Masonry Table Recipe", nameof(MasonryTableRecipe), typeof(MasonryTableItem))
        {
        }
    } 

    public partial class MasonryTableRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(MasonryTableRecipe), this.Ingredients);
        }
    }
    
    // needs crushedrock tag and sand added to returned items.
    [RequiresSkill(typeof (SurvivalistSkill), 0)]
    public class DisassemblyBloomeryRecipe : DefaultRecipeBuilder
    {
        public DisassemblyBloomeryRecipe() : base("Disassemble Bloomery Recipe", nameof(BloomeryRecipe), typeof(BloomeryItem))
        {
        }
    } 

    public partial class BloomeryRecipe
    {
        partial void ModsPostInitialize()
        {
            IngredientListStorage.OrigIngredientDictionary.Add(nameof(BloomeryRecipe), this.Ingredients);
        }
    }

    // [RequiresSkill(typeof (SurvivalistSkill), 0)]
    // public class DisassemblyRecipe : DefaultRecipeBuilder
    // {
    //     public DisassemblyRecipe() : base("Disassemble ", nameof(Recipe), typeof(Item))
    //     {
    //     }
    // }
}
