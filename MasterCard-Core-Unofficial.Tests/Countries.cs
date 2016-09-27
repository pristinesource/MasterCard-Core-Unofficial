/*
 * Copyright 2016 MasterCard International.
 *
 * Redistribution and use in source and binary forms, with or without modification, are 
 * permitted provided that the following conditions are met:
 *
 * Redistributions of source code must retain the above copyright notice, this list of 
 * conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list of 
 * conditions and the following disclaimer in the documentation and/or other materials 
 * provided with the distribution.
 * Neither the name of the MasterCard International Incorporated nor the names of its 
 * contributors may be used to endorse or promote products derived from this software 
 * without specific prior written permission.
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
 * SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
 * TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
 * OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER 
 * IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING 
 * IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF 
 * SUCH DAMAGE.
 *
 */

#if NOT_USED

using System;
using System.Collections.Generic;
using MasterCard.Core;
using MasterCard.Core.Exceptions;
using MasterCard.Core.Model;
using MasterCard.Core.Security;

namespace MasterCard.Api.Locations
{
    public class Countries : BaseObject
    {

        public Countries(BaseMap bm) : base(bm)
        {
		}

		private Countries(IDictionary<String,Object> map) : base(map)
		{
		}

        private Countries() : base()
        {
        }

        protected override string BasePath
        {
            get
            {
                return "/";
            }
        }

        protected override string ObjectType
        {
            get
            {
                return "atms/v1/country";
            }
        }

        
            

            

            

            

            
                
        /// <summary>
        /// Retrieves one object of type <code>Countries</code>
        /// </summary>
        /// <param name="id">The unique identifier which is used to identify an Countries object.</praram>
        /// <returns> A Countries object </returns>
        /// <exception cref="ApiCommunicationException"> </exception>
        /// <exception cref="AuthenticationException"> </exception>
        /// <exception cref="InvalidRequestException"> </exception>
        /// <exception cref="NotAllowedException"> </exception>
        /// <exception cref="ObjectNotFoundException"> </exception>
        /// <exception cref="SystemException"> </exception>
        public static Countries Read(String id)
        {
            BaseMap map = new BaseMap();
            map.Set("id", id);
            return (Countries) BaseObject.readObject(new Countries(map));
        }

            
        
            

            

            
                
        /// <summary>
        /// Retrieves one object of type <code>Countries</code>
        /// </summary>
        /// <returns> A Countries object </returns>
        /// <exception cref="ApiCommunicationException"> </exception>
        /// <exception cref="AuthenticationException"> </exception>
        /// <exception cref="InvalidRequestException"> </exception>
        /// <exception cref="NotAllowedException"> </exception>
        /// <exception cref="ObjectNotFoundException"> </exception>
        /// <exception cref="SystemException"> </exception>
        public static ResourceList<Countries> List()
        {
            return BaseObject.listObjects(new Countries());
        }


        /// <summary>
        /// Retrieves one object of type <code>Countries</code> using the specified criteria
        /// </summary>
        /// <param name="criteria">The criteria set of values which is used to identify the set of records of Countries object to return</praram>
        /// <returns>  a ResourceList<Countries> object which holds the list of Countries objects and the total number of Countries objects available. </returns>
        /// <exception cref="ApiCommunicationException"> </exception>
        /// <exception cref="AuthenticationException"> </exception>
        /// <exception cref="InvalidRequestException"> </exception>
        /// <exception cref="NotAllowedException"> </exception>
        /// <exception cref="ObjectNotFoundException"> </exception>
        /// <exception cref="SystemException"> </exception>
        public static ResourceList<Countries> List(BaseMap criteria)
        {
            return BaseObject.listObjects(new Countries(criteria));
        }
            

            

            
        
    }
}

#endif