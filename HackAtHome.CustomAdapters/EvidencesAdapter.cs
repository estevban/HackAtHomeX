using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HackAtHome.Entities;

namespace HackAtHome.CustomAdapters
{
    public  class EvidencesAdapter : BaseAdapter<Evidence>
    {

        Activity _context;
        int _idItemLayout;
        int _idLabItemDescription;
        int _idLabItemStatus;
        List<Evidence> _evidenceList;

        public EvidencesAdapter(Activity context, int idItemLayout, int idTitle, int idStatus, List<Evidence> evidenceList)
        {
            this._context = context;
            this._idItemLayout = idItemLayout;
            this._idLabItemDescription = idTitle;
            this._idLabItemStatus = idStatus;
            this._evidenceList = evidenceList;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return _evidenceList[position].EvidenceID;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _context.LayoutInflater.Inflate(_idItemLayout, parent, false);

            var evidenceItem = _evidenceList[position];

            var textViewLabItemDescription = view.FindViewById<TextView>(_idLabItemDescription);
            var textViewLabItemStatus = view.FindViewById<TextView>(_idLabItemStatus);

            textViewLabItemDescription.Text = evidenceItem.Title;
            textViewLabItemStatus.Text = evidenceItem.Status;

            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return _evidenceList.Count();
            }
        }

        public override Evidence this[int position] {
            get {
                return _evidenceList[position];
            }
        }
    }

    class EvidencesAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}