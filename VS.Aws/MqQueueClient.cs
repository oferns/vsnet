﻿namespace VS.Aws {

    using System;
    using System.Collections.Generic;
    using System.Text;
    using VS.Abstractions;
    using Amazon.MQ;
    using System.Threading.Tasks;

    public class MqQueueClient : IQueueClient {
        
        private readonly IAmazonMQ client;

        public MqQueueClient(IAmazonMQ client) {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }


        public async Task Publish() { 
            
        }

        public async Task Subscribe() {
        

        }

        public async Task UnSubscribe() { 
        
        }




    }
}