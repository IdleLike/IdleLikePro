using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace IdleLikeAdventureServer.Data.Entity{
	 	//Player
		public class Player
	{
	
      	/// <summary>
		/// ID
        /// </summary>
        public virtual int ID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// AccountID
        /// </summary>
        public virtual int AccountID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Name
        /// </summary>
        public virtual string Name
        {
            get; 
            set; 
        }        
		/// <summary>
		/// ServerID
        /// </summary>
        public virtual int ServerID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// UpdateDate
        /// </summary>
        public virtual DateTime UpdateDate
        {
            get; 
            set; 
        }        
		/// <summary>
		/// CreateDate
        /// </summary>
        public virtual DateTime CreateDate
        {
            get; 
            set; 
        }        
		   
	}
}