using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace IdleLikeAdventureServer.Data.Entity{
	 	//Account
		public class Account
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
		/// Name
        /// </summary>
        public virtual string Name
        {
            get; 
            set; 
        }        
		/// <summary>
		/// Password
        /// </summary>
        public virtual string Password
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