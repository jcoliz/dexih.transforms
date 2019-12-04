﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using dexih.functions;
using dexih.functions.Query;
using Dexih.Utils.DataType;

namespace dexih.transforms.Mapping
{
    public class MapColumn: Mapping
    {
        public MapColumn() {}

        public MapColumn(TableColumn inputColumn)
        {
            InputColumn = inputColumn;
            OutputColumn = inputColumn;
        }

        public MapColumn(TableColumn inputColumn, TableColumn outputColumn)
        {
            InputColumn = inputColumn;
            OutputColumn = outputColumn;
        }
        
        public MapColumn(object inputValue, TableColumn outputColumn)
        {
            InputValue = inputValue;
            OutputColumn = outputColumn;
        }

        public MapColumn(object inputValue, TableColumn inputColumn, TableColumn outputColumn)
        {
            InputValue = inputValue;
            InputColumn = inputColumn;
            OutputColumn = outputColumn;
        }

        
        public object InputValue;
        public TableColumn InputColumn;
        public TableColumn OutputColumn;

        protected int InputOrdinal = -1;
        protected int OutputOrdinal = -1;

        protected object[] RowData;

        public override void InitializeColumns(Table table, Table joinTable = null, Mappings mappings = null)
        {
            if (InputColumn == null) return;
            
            InputOrdinal = table.GetOrdinal(InputColumn);

            if (InputOrdinal >= 0 && InputColumn.DataType == ETypeCode.Node)
            {
                OutputColumn.ChildColumns = table[InputOrdinal].ChildColumns;
            }
            
            if (InputOrdinal < 0 && InputValue == null)
            {
                InputValue = InputColumn.DefaultValue;
            }
        }

        public override void AddOutputColumns(Table table)
        {
            OutputOrdinal = AddOutputColumn(table, OutputColumn);
        }

        public override Task<bool> ProcessInputRowAsync(FunctionVariables functionVariables, object[] row, object[] joinRow = null, CancellationToken cancellationToken = default)
        {
            RowData = row;
            return Task.FromResult(true);
        }
        
        public override void MapOutputRow(object[] data)
        {
            data[OutputOrdinal] = GetOutputValue();
        }
        
        public override object GetOutputValue(object[] row = null)
        {
            object returnValue;
            if (InputOrdinal == -1 )
            {
                returnValue = InputValue;
                return Operations.Parse(OutputColumn.DataType, returnValue);
            }
            else
            {
                returnValue = row == null ? RowData?[InputOrdinal] : row[InputOrdinal];
                return InputColumn.DataType == OutputColumn.DataType ? returnValue : Operations.Parse(OutputColumn.DataType, returnValue);
            }
        }

        public override string Description()
        {
            return $"Mapping ({InputColumn?.Name} => {OutputColumn?.Name}";
        }

        public override void Reset(EFunctionType functionType)
        {
        }
        
        public override IEnumerable<SelectColumn> GetRequiredColumns(bool includeAggregate)
        {
            return new []{new SelectColumn(InputColumn)};
        }


    }
}