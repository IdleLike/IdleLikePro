using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace IdleLikeAdventureServer.Data.Entity{
	 	//Actor
		public class Actor
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
		/// PlayerID
        /// </summary>
        public virtual int PlayerID
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
		/// RaceID
        /// </summary>
        public virtual int RaceID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// CareerID
        /// </summary>
        public virtual int CareerID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// CareerLevel
        /// </summary>
        public virtual int CareerLevel
        {
            get; 
            set; 
        }        
		/// <summary>
		/// CareerPoint
        /// </summary>
        public virtual int CareerPoint
        {
            get; 
            set; 
        }        
		/// <summary>
		/// TotalExp
        /// </summary>
        public virtual int TotalExp
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