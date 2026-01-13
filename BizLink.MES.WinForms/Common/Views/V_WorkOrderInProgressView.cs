using AntdUI;
using BizLink.MES.Domain.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.WinForms.Common.Views
{
    public class V_WorkOrderInProgressView : AntdUI.NotifyProperty
    {
        public V_WorkOrderInProgressView(V_WorkOrderInProgress entity)
        {
            _orderId = entity.OrderId;
            _taskId = entity.TaskId;
            _orderProcessId = entity.OrderProcessId;
            _prevProcessId = entity.PrevProcessId;
            _profitCenter = entity.ProfitCenter;
            _orderNumber = entity.OrderNumber;
            _nextWorkCenter = entity.NextWorkCenter;
            _operation = entity.Operation;
            _materialCode = entity.MaterialCode;
            _materialDesc = entity.MaterialDesc;
            _quantity = entity.Quantity;
            _cablePcs = entity.CablePcs;
            _cableItem = entity.CableItem;
            _cableMaterial = entity.CableMaterial;
            _cableMaterialDesc = entity.CableMaterialDesc;
            _completedQty = entity.CompletedQty;
            _cableLength = entity.CableLength;
            _cableLengthUsl = entity.CableLengthUsl;
            _cableLengthDsl = entity.CableLengthDsl;
            _dispatchDate = entity.DispatchDate;
            _startTime = entity.StartTime;
            _status = entity.Status;
            _plannerRemark = entity.PlannerRemark;
            _workCenter = entity.WorkCenter;
            _progress = new CellProgress((float)(entity.Quantity == 0 ? 0 : entity.CompletedQty / entity.Quantity));

        }

        private CellTag BuildStatusTag(string status)
        {
            switch (status)
            {
                case "0":
                    return new AntdUI.CellTag("未准备", AntdUI.TTypeMini.Error);
                case "1":
                    return new AntdUI.CellTag("已排产", AntdUI.TTypeMini.Default);

                case "2":
                    return new AntdUI.CellTag("执行中", AntdUI.TTypeMini.Info);

                case "3":
                    return new AntdUI.CellTag("已挂起", AntdUI.TTypeMini.Warn);

                case "4":
                    return new AntdUI.CellTag("已完成", AntdUI.TTypeMini.Success);

                default:
                    return new AntdUI.CellTag("未知", AntdUI.TTypeMini.Default);
            }
        }

        int _orderId;
        public int OrderId
        {
            get => _orderId;
            set
            {
                if (_orderId == value)
                    return;
                _orderId = value;
                OnPropertyChanged();
            }
        }

        int? _taskId;
        public int? TaskId
        {
            get => _taskId;
            set
            {
                if (_taskId == value)
                    return;
                _taskId = value;
                OnPropertyChanged();
            }
        }

        int _orderProcessId;
        public int OrderProcessId
        {
            get => _orderProcessId;
            set
            {
                if (_orderProcessId == value)
                    return;
                _orderProcessId = value;
                OnPropertyChanged();
            }
        }


        int? _prevProcessId;
        public int? PrevProcessId
        {
            get => _prevProcessId;
            set
            {
                if (_prevProcessId == value)
                    return;
                _prevProcessId = value;
                OnPropertyChanged();
            }
        }

        string? _profitCenter;
        public string? ProfitCenter
        {
            get => _profitCenter;
            set
            {
                if (_profitCenter == value)
                    return;
                _profitCenter = value;
                OnPropertyChanged();
            }
        }
        string? _orderNumber;
        public string? OrderNumber
        {
            get => _orderNumber;
            set
            {
                if (_orderNumber == value)
                    return;
                _orderNumber = value;
                OnPropertyChanged();
            }
        }
        string? _materialCode;
        public string? MaterialCode
        {
            get => _materialCode;
            set
            {
                if (_materialCode == value)
                    return;
                _materialCode = value;
                OnPropertyChanged();
            }
        }

        string? _operation;
        public string? Operation
        {
            get => _operation;
            set
            {
                if (_operation == value)
                    return;
                _operation = value;
                OnPropertyChanged();
            }
        }

        string? _productCode;
        public string? ProductCode
        {
            get => _productCode;
            set
            {
                if (_productCode == value)
                    return;
                _productCode = value;
                OnPropertyChanged();
            }
        }

        string? _nextWorkCenter;
        public string? NextWorkCenter
        {
            get => _nextWorkCenter;
            set
            {
                if (_nextWorkCenter == value)
                    return;
                _nextWorkCenter = value;
                OnPropertyChanged();
            }
        }
        string? _materialDesc;
        public string? MaterialDesc
        {
            get => _materialDesc;
            set
            {
                if (_materialDesc == value)
                    return;
                _materialDesc = value;
                OnPropertyChanged();
            }
        }
        decimal? _quantity;
        public decimal? Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity == value)
                    return;
                _quantity = value;
                OnPropertyChanged();
            }
        }

        int? _cablePcs;
        public int? CablePcs
        {
            get => _cablePcs;
            set
            {
                if (_cablePcs == value)
                    return;
                _cablePcs = value;
                OnPropertyChanged();
            }
        }

        string? _cableMaterial;
        public string? CableMaterial
        {
            get => _cableMaterial;
            set
            {
                if (_cableMaterial == value)
                    return;
                _cableMaterial = value;
                OnPropertyChanged();
            }
        }

        string? _cableMaterialDesc;
        public string? CableMaterialDesc
        {
            get => _cableMaterialDesc;
            set
            {
                if (_cableMaterialDesc == value)
                    return;
                _cableMaterialDesc = value;
                OnPropertyChanged();
            }
        }

        string? _cableItem;
        public string? CableItem
        {
            get => _cableItem;
            set
            {
                if (_cableItem == value)
                    return;
                _cableItem = value;
                OnPropertyChanged();
            }
        }

        decimal? _completedQty;
        public decimal? CompletedQty
        {
            get => _completedQty;
            set
            {
                if (_completedQty == value)
                    return;
                _completedQty = value;
                OnPropertyChanged();
            }
        }
        decimal? _cableLength;
        public decimal? CableLength
        {
            get => _cableLength;
            set
            {
                if (_cableLength == value)
                    return;
                _cableLength = value;
                OnPropertyChanged();
            }
        }
        decimal? _cableLengthUsl;
        public decimal? CableLengthUsl
        {
            get => _cableLengthUsl;
            set
            {
                if (_cableLengthUsl == value)
                    return;
                _cableLengthUsl = value;
                OnPropertyChanged();
            }
        }
        decimal? _cableLengthDsl;
        public decimal? CableLengthDsl
        {
            get => _cableLengthDsl;
            set
            {
                if (_cableLengthDsl == value)
                    return;
                _cableLengthDsl = value;
                OnPropertyChanged();
            }
        }
        DateTime? _dispatchDate;
        public DateTime? DispatchDate
        {
            get => _dispatchDate;
            set
            {
                if (_dispatchDate == value)
                    return;
                _dispatchDate = value;
                OnPropertyChanged();
            }
        }
        DateTime? _startTime;
        public DateTime? StartTime
        {
            get => _startTime;
            set
            {
                if (_startTime == value)
                    return;
                _startTime = value;
                OnPropertyChanged();
            }
        }
        string _status;
        public string Status
        {
            get => _status;
            set
            {
                if (_status == value)
                    return;
                _status = value;
                OnPropertyChanged();
            }
        }
        string? _plannerRemark;
        public string? PlannerRemark
        {
            get => _plannerRemark;
            set
            {
                if (_plannerRemark == value)
                    return;
                _plannerRemark = value;
                OnPropertyChanged();
            }
        }

        string? _workCenter;
        public string? WorkCenter
        {
            get => _workCenter;
            set
            {
                if (_workCenter == value)
                    return;
                _workCenter = value;
                OnPropertyChanged();
            }
        }

        AntdUI.CellProgress _progress;
        public AntdUI.CellProgress Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged();
            }
        }
    }
}
