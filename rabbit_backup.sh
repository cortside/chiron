curl -u admin:password -X GET http://127.0.0.1:15672/api/definitions > rabbit_chiron.json


curl -u admin:password -X POST -H "content-type:application/json" -d @rabbit_chiron.json http://127.0.0.1:15672/api/definitions

curl -X POST --header 'Content-Type: application/json-patch+json' -d '{ \ 
   "firstName": "string", \ 
   "lastName": "string", \ 
   "email": "string", \ 
   "password": "string", \ 
   "phoneNumber": "string", \ 
   "agreeToTerms": true \ 
 }' 'http://localhost:5001/api/Register'
 
 