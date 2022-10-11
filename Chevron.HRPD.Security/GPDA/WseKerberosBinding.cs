using System;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using System.Xml;

namespace Chevron.HRPD.Security
{
    class WseKerberosBinding : Binding
    {
        public override BindingElementCollection CreateBindingElements()
        {
            BindingElementCollection bec = new BindingElementCollection();

            SymmetricSecurityBindingElement securityBinding;

            BindingElement transport;

            transport = new HttpTransportBindingElement();

            securityBinding = (SymmetricSecurityBindingElement)SecurityBindingElement.CreateKerberosBindingElement();

            securityBinding.RequireSignatureConfirmation = false;

            securityBinding.MessageProtectionOrder = MessageProtectionOrder.SignBeforeEncrypt;

            securityBinding.IncludeTimestamp = true;

            securityBinding.SetKeyDerivation(false);

            bec.Add(securityBinding);

            TextMessageEncodingBindingElement textElement = new TextMessageEncodingBindingElement();

            textElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.Soap11WSAddressingAugust2004;

            bec.Add(textElement);

            bec.Add(transport);

            return bec;
            
        }

        public override string Scheme
        {
            get
            {
                return "http";
            }
        }
    }
}
