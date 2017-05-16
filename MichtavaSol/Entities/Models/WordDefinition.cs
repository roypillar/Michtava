using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{

    public class WordDefinition : DeletableEntity
    {

        public string _definition;

        public WordDefinition()
        {
            _definition = "";
        }

        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public Guid Id { get; set; }

        [Required,Key]
        public string Word { get; set; }

        //public string Definition { get; set; }
            
        public string Definition
        {
            get
            {
                return this._definition;
            }
            set
            {
                this._definition = value;
            }
        }
        public List<string> getDefinitions()//can only access definitions from this method.
        {
            return new List<string>(_definition.Split(GlobalConstants.DEFINITION_SEPERATOR));
        }

        public void addDefinition(string def)
        {
            _definition += GlobalConstants.DEFINITION_SEPERATOR + def.Trim();
        }

        public void addDefinitions(List<string> defs)
        {
            foreach(string def in defs)
            {
                if (_definition == "")
                    _definition += def.Trim();
                else
                    _definition += GlobalConstants.DEFINITION_SEPERATOR + def.Trim();
            }
        }

        public override void setId(Guid id)
        {
        }

    }
}
