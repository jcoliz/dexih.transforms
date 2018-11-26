﻿using System.Threading.Tasks;
using dexih.functions.Query;

namespace dexih.functions.Mappings
{
    public class MapSort: Mapping
    {
       
        public MapSort(TableColumn inputColumn, Sort.EDirection direction)
        {
            InputColumn = inputColumn;
            Direction = direction;
        }
        
        public Sort.EDirection Direction;
        public object InputValue;
        public TableColumn InputColumn;

        private int _inputOrdinal = -1;

        private object[] _rowData;

        public override void InitializeColumns(Table table, Table joinTable = null)
        {
            if (InputColumn != null)
            {
                _inputOrdinal = table.GetOrdinal(InputColumn);
                if (_inputOrdinal < 0 && InputValue == null)
                {
                    InputValue = InputColumn.DefaultValue;
                }
            }
        }

        public override void AddOutputColumns(Table table)
        {
        }

        public override Task<bool> ProcessInputRow(FunctionVariables functionVariables, object[] row, object[] joinRow = null)
        {
            _rowData = row;
            return Task.FromResult(true);
        }

        public override void MapOutputRow(object[] data) 
        {
        }

        public override object GetInputValue(object[] row = null)
        {
            if (_inputOrdinal == -1)
            {
                return InputValue;
            }
            else
            {
                return row == null ? row[_inputOrdinal] : _rowData[_inputOrdinal];     
            }        
        }

        public override string Description()
        {
            return $"Sort {InputColumn?.Name} {Direction}";
        }

        public override void Reset(EFunctionType functionType)
        {
            _rowData = null;
        }

    }
}